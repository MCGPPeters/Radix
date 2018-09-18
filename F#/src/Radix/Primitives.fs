namespace Radix

open System.IO
open Radix.Routing.Types
open Radix.Routing.Implementation
open System

module Primitives =   

    type Behavior<'state, 'message> = 'state -> 'message -> 'state

    type InitialState<'s> = InitialState of 's

    type Create<'state, 'message> = Behavior<'state, 'message> -> 'state -> Address<'message>

    type Send<'message> = Address<'message> -> 'message -> unit

    type Receive<'message> = Envelope<'message> -> unit

    type RegisterAgent<'message> = {
        Address: Address<'message>
        Agent: Agent<'message>
    }
    
    type AgentRegistered<'message> = {
        Address: Address<'message>
        Agent: Agent<'message>
    }

    type AgentRegisteredEvent<'message> = Event<AgentRegistered<'message>>
    
    type RegisterAgentCommand<'message> = Command<RegisterAgent<'message>>

    type NodeMessage<'message> =
    | Envelope of Envelope<'message>
    | RegisterAgent of RegisterAgentCommand<'message> * AsyncReplyChannel<AgentRegisteredEvent<'message>> 

    type Primitives<'state, 'message> = {
        Create: Create<'state, 'message>
        Receive: Receive<'message>
    }        


    let inline (<--) (address: Address<'message>) (message: 'message) = address.Send message

    module Node = 

        let create (deserialize: Deserialize<'message>) (serialize: Serialize<'message>) (resolveRemoteAddress: ResolveRemoteAddress<'message>) (forward: Forward<'message>) =
            
            let registry = Registry (Map.empty)
            
            let node : MailboxProcessor<NodeMessage<'message>> = MailboxProcessor.Start(fun inbox ->
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

                        let newState = state.Add (command.Payload.Address, command.Payload.Agent)
   
                        let agentRegistered: AgentRegisteredEvent<'message> = {
                            Payload = { 
                                Address = command.Payload.Address
                                Agent = command.Payload.Agent}
                            Timestamp = DateTimeOffset.Now
                        }
                        replyChannel.Reply agentRegistered
                        return! messageLoop (Registry newState)
                        
                }

                messageLoop registry
            )

            

            let create : Create<'state, 'message> = fun behavior initialState ->
                let agent = MailboxProcessor.Start(fun inbox ->
                    let rec messageLoop state = async {
                        let! message = inbox.Receive()
                        let newState = behavior state message
                        return! messageLoop newState                              
                    }

                    messageLoop initialState
                )                  

                let guid = Guid.NewGuid()

                let address = 
                    {  new Address<'message>(guid) with            
                            member this.Send m = 
                                let envelope : Envelope<'message> = {
                                    Payload = (Message m)
                                    Destination = this
                                    Principal = Threading.Thread.CurrentPrincipal                 
                                }
                                node.Post (Envelope envelope)}

                let registerAgentCommand: RegisterAgentCommand<'message> = {
                    Payload = {Agent = Agent agent
                               Address = address}
                    Timestamp = DateTimeOffset.Now
                    Principal = Threading.Thread.CurrentPrincipal 
                }
                let asyncReply = node.PostAndAsyncReply (fun channel ->  RegisterAgent (registerAgentCommand, channel))
                let agentRegistered = Async.RunSynchronously asyncReply
                agentRegistered.Payload.Address

            {
                Create = create
                Receive = fun envelope ->
                    node.Post (Envelope envelope)
            }            