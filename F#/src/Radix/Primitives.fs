namespace Radix

open System.IO
open Radix.Routing.Types
open Radix.Routing.Implementation
open System

module Primitives =   

    type Behavior<'state, 'message> = 'state -> 'message -> 'state

    type InitialState<'s> = InitialState of 's

    type Create<'state, 'message> = Behavior<'state, 'message> -> 'state -> Address

    type Send<'message> = Address -> 'message -> unit

    type Receive<'message> = Envelope<'message> -> unit

    type RegisterAgent<'message> = {
        Agent: Agent<'message>
    }
    
    type AgentRegistered = Event<Address>
    
    type RegisterAgentCommand<'message> = Command<RegisterAgent<'message>>

    type NodeMessage<'message> =
    | Envelope of Envelope<'message>
    | RegisterAgent of RegisterAgentCommand<'message> * AsyncReplyChannel<AgentRegistered> 

    type Node<'message> = Node of MailboxProcessor<NodeMessage<'message>>

    type Primitives<'state, 'message> = {
        Send: Send<'message>
        Create: Create<'state, 'message>
        Receive: Receive<'message>
    }        

    module Node = 

        let create (deserialize: Deserialize<'message>) (serialize: Serialize<'message>) (resolveRemoteAddress: ResolveRemoteAddress<'message>) (forward: Forward<'message>) =
            
            let registry = Registry (Map.empty)
            
            let node = MailboxProcessor.Start(fun inbox ->
                let rec messageLoop (Registry state) = async {
                    let! message = inbox.Receive()

                    match message with
                    | Envelope envelope ->
                        let! deliveryResult = route resolveRemoteAddress (Registry state)  serialize deserialize forward envelope

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
                    Payload = (Message message)
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
                Receive = fun envelope ->
                    node.Post (Envelope envelope)
            }            