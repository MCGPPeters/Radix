//namespace Radix

//open System.IO
//open Radix.Routing.Types
//open Radix.Routing.Implementation
//open System

//module Primitives =   

//    type Behavior<'state, 'message> = ^state -> ^message -> ^state

//    type InitialState<'s> = InitialState of 's

    

//    type Send<'message> = Address -> 'message -> unit

//    type Receive<'message> = Envelope<'message> -> unit

//    type RegisterAgent<'message> = {
//        Address: Address
//        Agent: Agent<'message>
//    }
    
//    type AgentRegistered<'message> = {
//        Address: Address
//        Agent: Agent<'message>
//    }
    
//    type NodeMessage =
//    | Envelope of Envelope
//    | RegisterAgent of RegisterAgent<'message> * AsyncReplyChannel<AgentRegistered<'message>> 

//    type Node< ^message> = MailboxProcessor<NodeMessage>

//    type Create< ^state, ^message> = Behavior< ^state, ^message> -> ^state -> Address

//    type Primitives< ^state, ^message> = {
//        Send: Send< ^message>
//        Create: Create< ^state, ^message>
//        Receive: Receive< ^message>
//    }
    
   


//    module Node = 

//        let inline create (deserialize: Deserialize< ^message>) (serialize: Serialize< ^message>) (resolveRemoteAddress: ResolveRemoteAddress< ^message>) (forward: Forward< ^message>) =
            
//            let registry = Registry (Map.empty)
            
//            let node : MailboxProcessor<NodeMessage<'message>> = MailboxProcessor.Start(fun inbox ->
//                let rec messageLoop (Registry state) = async {
//                    let! message = inbox.Receive()

//                    match message with
//                    | Envelope envelope ->
//                        let! deliveryResult = route resolveRemoteAddress (Registry state)  serialize deserialize forward envelope

//                        //match deliveryResult with
//                        //| Ok (EnvelopePosted posted) ->  posted //logging
//                        //| Ok (EnvelopeForwarded forwarded) ->  forwarded //logging
//                        return! messageLoop (Registry state)
//                    | RegisterAgent (command, replyChannel) ->

//                        let newState = state.Add (command.Address, command.Agent)
   
//                        let agentRegistered: AgentRegistered<'message> = {
//                                 Address = command.Address
//                                 Agent = command.Agent
//                        }
//                        replyChannel.Reply agentRegistered
//                        return! messageLoop (Registry newState)
                        
//                }

//                messageLoop registry
//            )

            

//            let inline create (behavior: Behavior< ^state, ^message>) initialState =
//                let agent: MailboxProcessor<'message> = MailboxProcessor.Start(fun inbox ->
                    
//                    let inline send address (message: ^message) = 
//                        let envelope  = {
//                            Payload = (Message message)
//                            Destination = address
//                            Principal = Threading.Thread.CurrentPrincipal                 
//                        }
//                        node.Post (Envelope envelope)

//                    let rec messageLoop state = async {
//                        let! message = inbox.Receive()
//                        let newState = behavior state message
//                        return! messageLoop newState                              
//                    }

//                    messageLoop initialState
//                )                  

//                let guid = Guid.NewGuid()
//                let address = Address.create guid

//                let registerAgentCommand: RegisterAgent<'message> = {
//                    Agent = Agent agent
//                    Address = address
//                }

//                let agentRegistered = 
//                    node.PostAndAsyncReply (fun channel ->  RegisterAgent (registerAgentCommand, channel))
//                    |> Async.RunSynchronously

//                agentRegistered.Address

//            let inline send address (message: ^message) = 
//                    let envelope  = {
//                        Payload = (Message message)
//                        Destination = address
//                        Principal = Threading.Thread.CurrentPrincipal                 
//                    }
//                    node.Post (Envelope envelope)
//            {
//                Send = (<--)
//                Create = create
//                Receive = fun envelope ->
//                    node.Post (Envelope envelope)
//            }