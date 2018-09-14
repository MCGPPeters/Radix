namespace Radix

open System.IO
open Radix.Routing.Types
open Radix.Routing.Implementation
open System

module Primitives =

    type DeserializationError = DeserializationError of string

    type SerializationError = SerializationError of string

    type Serialize<'message> = 'message -> Stream -> unit

    type Behavior<'state, 'message> = 'state -> 'message -> 'state

    type InitialState<'s> = InitialState of 's

    type Create<'state, 'message> = Behavior<'state, 'message> -> 'state -> Address

    type Send<'message> = Address -> 'message -> unit

    let post: Post<'message> =
                fun (Registry registry') deserialize envelope -> 
                    let (Agent entry) = registry'.Item envelope.Destination
                    entry.Post envelope.Payload
                    {
                        Aggregate = envelope.Destination
                        Timestamp = DateTimeOffset.Now
                        Payload = envelope
                    } 

    type RegisterAgent<'message> = {
        Agent: Agent<'message>
    }
    
    type AgentRegistered = Event<Address>
    
    type RegisterAgentCommand<'message> = Command<RegisterAgent<'message>>

    type NodeMessage<'message> =
    | Envelope of Envelope<'message>
    | RegisterAgent of RegisterAgentCommand<'message> * AsyncReplyChannel<AgentRegistered> 

    type Node<'message> = Node of MailboxProcessor<NodeMessage<'message>>

    type SendMessage<'message> = Node<'message> -> Serialize<'message> -> Address -> 'message -> Async<unit>
    
    type Primitives<'state, 'message> = {
        Send: Send<'message>
        Create: Create<'state, 'message>
    }        

    module Node = 

        let create registry (deserialize: Deserialize<'message>) (serialize: Serialize<'message>) (resolveRemoteAddress: ResolveRemoteAddress<'message>) (forward: Forward) =
            
            let node = MailboxProcessor.Start(fun inbox ->
                let rec messageLoop (Registry state) = async {
                    let! message = inbox.Receive()

                    match message with
                    | Envelope envelope ->
                        let! deliveryResult = route (resolveLocalAddress (Registry state)) resolveRemoteAddress (Registry state) forward deserialize post envelope

                        //match deliveryResult with
                        //| Ok (EnvelopePosted posted) ->  posted //logging
                        //| Ok (EnvelopeForwarded forwarded) ->  forwarded //logging
                        return! messageLoop (Registry state)
                    | RegisterAgent (command, replyChannel) ->
                        let address = Address.create (Guid.NewGuid())
                        let newState = state.Add (address, command.Payload.Agent)
                       
                        
                        let agentRegistered: AgentRegistered = {
                            Payload = address
                            Timestamp = DateTimeOffset.Now
                            Aggregate = Address.create (Guid.NewGuid())//???
                        }
                        replyChannel.Reply agentRegistered
                        return! messageLoop (Registry newState)
                        
                }

                messageLoop registry
            )

            let send : Send<'message> = fun address message ->

                let envelope : Envelope<'message> = {
                    Payload = message
                    Destination = address
                    Principal = Threading.Thread.CurrentPrincipal                 
                }
                node.Post (Envelope envelope)

            let create : Create<'state, 'message> = fun behavior initialState ->
                let agent = MailboxProcessor.Start(fun inbox ->
                    let rec messageLoop state = async {
                        let! message = inbox.Receive()
                        let newState = behavior state message
                        return! messageLoop newState                              
                    }

                    messageLoop initialState
                )

                let registerAgentCommand: RegisterAgentCommand<'message> = {
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