module Root

open System.IO
open System
open System.Security.Cryptography

type Undefined = exn

type AsyncResult<'success, ' failure> = Async<Result<'success, 'failure>>

type Hash = Hash of byte[]

type Address = Address of Hash

module Address = 

    let create (guid: Guid) = 
        let sha1 = SHA1.Create()
        Address (Hash (sha1.ComputeHash(guid.ToByteArray())))

type ValidationError = Undefined

type Validated<'t> = Validated of 't

type Validate<'t> = 't -> Result<Validated<'t>, ValidationError>

type Map<'a, 'b> = Validated<'a> -> 'b



type Envelope =
    abstract Open<'message> : unit -> 'message
    abstract Address: Address
and Envelope<'message> = { 
        Message : 'message
        Address: Address }
with interface Envelope with
        member this.Open<'message> () = this.Message
        member this.Address = this.Address

type DTO<'payload> = {
    Address : Address
    Payload : 'payload
    }

module TrustBoundary =

    type Deserialize<'payload> = Stream -> DTO<'payload>

    type Listen = unit -> Stream

    type Pack<'message> = Envelope<'message> -> Envelope

    type Input<'payload, 'message> = Deserialize<'payload> -> Validate<'payload> -> Map<'payload, 'message> -> Pack<'message> -> Result<Envelope, ValidationError>

    type Serialize<'message> = Envelope<'message> -> MemoryStream -> Stream

    type Output<'message, 'payload> = Envelope<'message> -> DTO<'payload> -> Serialize<'message>

open TrustBoundary

module Routing =

    type AddressNotFoundError = AddressNotFoundError of string

    type Resolve = Address -> AsyncResult<Uri, AddressNotFoundError>

    type EnvelopePosted = Event<Envelope>

    type EnvelopeForwarded = Event<Envelope>

    type Agent = Agent of MailboxProcessor<Envelope>

    type Registry = Registry of FSharp.Collections.Map<Address, Agent>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type Forward = Uri -> Envelope -> AsyncResult<EnvelopeForwarded, UnableToDeliverEnvelopeError>

open Routing

type RegisterAgent = {
        Agent: Agent
    }
    
type AgentRegistered = {
        Address: Address
    }
    
type ContextCommand =
    | Accept of Envelope
    | RegisterAgent of RegisterAgent * AsyncReplyChannel<AgentRegistered> 
    
type BoundedContext = BoundedContext of MailboxProcessor<ContextCommand>

module Actor = 

    type Behavior<'state, 'message> = ^state -> ^message -> ^state

    type Create< ^state, ^message> = Behavior< ^state, ^message> -> ^state -> Address



module BoundedContext =

    open Actor

    let create (resolve : Resolve) (forward: Forward) (Registry registry) =

        let context = BoundedContext (MailboxProcessor.Start(fun inbox -> 
                let rec messageLoop (Registry state) = async {
                    let! command = inbox.Receive()
                    match command with
                    | Accept envelope ->
                        match registry.TryFind (envelope.Address) with
                        | Some (Agent agent) -> 
                            agent.Post envelope
                        | _ -> 
                            let! resolveResult = resolve (envelope.Address)
                            match resolveResult with
                            | Ok uri-> 
                                let! forwardResult = forward uri envelope
                                forwardResult |> ignore // log
                                return! messageLoop (Registry state)
                            | Error error -> 
                                error |> ignore //log
                                return! messageLoop (Registry state)
                     | RegisterAgent (command, replyChannel) ->

                        let guid = Guid.NewGuid()

                        let address = Address.create guid
                        let newState = state.Add (address, command.Agent)
   
                        let agentRegistered: AgentRegistered = {
                                 Address = address
                        }
                        replyChannel.Reply agentRegistered
                        return! messageLoop (Registry newState)

                    return! messageLoop (Registry state)
                }
                    
                messageLoop (Registry registry)
                    
            )
        )

        let (BoundedContext mailboxProcessor) = context
        let inline pack (envelope: Envelope<'message>) = envelope :> Envelope

        let inline send (address: Address) (message: ^m) =
            {   
                Address = address
                Message = message }
            |> pack
            |> Accept 
            |> mailboxProcessor.Post 



        let inline create (BoundedContext context): Create< ^s, ^m> = fun (behavior: Behavior< ^s, ^m>) initialState ->
            let agent: MailboxProcessor<Envelope> = 
                    MailboxProcessor.Start(fun inbox ->
                        let rec inline messageLoop state = async {
                            let! envelope = inbox.Receive()

                            let message = envelope.Open()
                        
                            let newState = behavior state message

                            return! messageLoop newState                              
                        }

                        messageLoop initialState
                    )   
            let registerAgentCommand: RegisterAgent = {
                Agent = Agent agent
            }

            let agentRegistered = 
                context.PostAndAsyncReply (fun channel ->  RegisterAgent (registerAgentCommand, channel))
                |> Async.RunSynchronously

            agentRegistered.Address

        send

module IO = 

    open TrustBoundary

    type PayloadAccepted<'payload> = Event<'payload>

    type PayloadDeclined<'payload> = Event<'payload>

    type Accept<'payload, 'message> = Input<'payload, 'message> -> BoundedContext -> Result<PayloadAccepted<'payload>, PayloadDeclined<'payload>>

    type Publish<'payload, 'message> = Output<'message, 'payload> -> Envelope<'message> -> unit


