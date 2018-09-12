namespace Radix

open System.IO
open Radix.Routing.Types
open Radix.Routing.Implementation
open System

module Primitives =

    type DeserializationError = DeserializationError of string

    type Deserialize<'message> = Stream -> AsyncResult<'message, DeserializationError>

    type SerializationError = SerializationError of string

    type Serialize<'message> = 'message -> AsyncResult<Stream, SerializationError>

    type Behavior<'state, 'message> = 'state -> 'message -> Async<'state>

    type InitialState<'s> = InitialState of 's

    type Create<'state, 'message> = Behavior<'state, 'message> -> InitialState<'state> -> Address

    type Send<'message> = Address -> 'message -> unit

    let post: Post =
                fun (Registry registry') envelope -> 
                    let (Agent entry) = registry'.Item envelope.Destination
                    entry.Post envelope.Payload
                    {
                        Aggregate = envelope.Destination
                        Timestamp = DateTimeOffset.Now
                        Payload = envelope
                    } 

    type RegisterAgent = {
        Agent: Agent
    }
    
    type AgentRegistered = Event<Address>
    
    type RegisterAgentCommand = Command<RegisterAgent>

    type NodeMessage =
    | Envelope of Envelope
    | RegisterAgent of RegisterAgentCommand * AsyncReplyChannel<AgentRegistered>    

    type Node = Node of MailboxProcessor<NodeMessage>

    type SendMessage<'message> = Node -> Serialize<'message> -> Address -> 'message -> Async<unit>
    
    type Primitives<'state, 'message> = {
        Send: Send<'message>
        Create: Create<'state, 'message>
    }        

    module Node = 

        let create registry deserialize serialize (resolveRemoteAddress: ResolveRemoteAddress) (forward: Forward) =
            
            let node = MailboxProcessor.Start(fun inbox ->
                let rec messageLoop (Registry state) = async {
                    let! message = inbox.Receive()

                    match message with
                    | Envelope envelope ->
                        asyncResult {
                            let! deliveryResult = route (resolveLocalAddress registry) resolveRemoteAddress registry forward post envelope
                            match deliveryResult with
                            | EnvelopePosted posted ->  posted |> ignore //logging
                            | EnvelopeForwarded forwarded ->  forwarded |> ignore //logging
                        } |> ignore
                        return! messageLoop (Registry state)
                    | RegisterAgent (command, replyChannel) ->
                        let address = Address.create
                        let newState = state.Add (address, command.Payload.Agent)
                        return! messageLoop (Registry newState)
                        
                        let agentRegistered: AgentRegistered = {
                            Payload = address
                            Timestamp = DateTimeOffset.Now
                            Aggregate = Address.create //???
                        }
                        replyChannel.Reply agentRegistered
                }

                messageLoop registry
            )

            let send : Send<'message> = fun address message ->
                let stream = serialize message
                let envelope : Envelope = {
                    Payload = stream
                    Destination = address
                    Principal = Threading.Thread.CurrentPrincipal                 
                }
                node.Post (Envelope envelope)

            let create : Create<'state, 'message> = fun behavior (InitialState initialState) ->
                let agent = MailboxProcessor.Start(fun inbox ->
                    let rec messageLoop state = async {
                        let! stream = inbox.Receive()
                        let! deserializationResult = deserialize stream
                        match deserializationResult with
                        | Ok message -> 
                            let! newState = behavior state message
                            return! messageLoop newState
                        | Error error -> //log
                            return! messageLoop state                                 
                    }

                    messageLoop initialState
                )

                let registerAgentCommand: RegisterAgentCommand = {
                    Payload = {Agent = Agent agent}
                    Timestamp = DateTimeOffset.Now
                    Principal = Threading.Thread.CurrentPrincipal 
                }
                let asyncReply = node.PostAndAsyncReply (fun channel ->  RegisterAgent (registerAgentCommand, channel))
                let agentRegistered = Async.RunSynchronously asyncReply
                agentRegistered.Payload

            {
                Create = create
                Send = send
            }            