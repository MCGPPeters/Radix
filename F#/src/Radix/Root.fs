namespace Root

open System.IO
open System
open System.Security.Cryptography

type Undefined = exn

[<RequireQualifiedAccess>] 
module Async =
    let retn x = 
        async.Return x

    let map f xA = 
        async { 
        let! x = xA
        return f x 
        }    

type AsyncResult<'success, ' failure> = Async<Result<'success, 'failure>>

[<RequireQualifiedAccess>] 
module AsyncResult = 

    let map f (x:AsyncResult<_,_>) : AsyncResult<_,_> =
        Async.map (Result.map f) x

    let mapError f (x:AsyncResult<_,_>) : AsyncResult<_,_> =
        Async.map (Result.mapError f) x    

    let ofSuccess x : AsyncResult<_,_> = 
        x |> Result.Ok |> Async.retn

    /// Lift a value into an Error inside a AsyncResult
    let ofError x : AsyncResult<_,_> = 
        x |> Result.Error |> Async.retn 

    /// Lift a Result into an AsyncResult
    let ofResult x : AsyncResult<_,_> = 
        x |> Async.retn

    /// Lift a Async into an AsyncResult
    let ofAsync x : AsyncResult<_,_> = 
        x |> Async.map Result.Ok

    let retn x : AsyncResult<_,_> = 
        x |> Result.Ok |> Async.retn

    let bind (f: 'a -> AsyncResult<'b,'c>) (xAsyncResult : AsyncResult<_, _>) :AsyncResult<_,_> = async {
        let! xResult = xAsyncResult 
        match xResult with
        | Ok x -> return! f x
        | Error err -> return (Error err)
        }    

// ==================================
// AsyncResult computation expression
// ==================================

/// The `asyncResult` computation expression is available globally without qualification
[<AutoOpen>]
module AsyncResultComputationExpression = 

    type AsyncResultBuilder() = 
        member __.Return(x) = AsyncResult.retn x
        member __.Bind(x, f) = AsyncResult.bind f x

        member __.ReturnFrom(x) = x
        member this.Zero() = this.Return ()

        member __.Delay(f) = f
        member __.Run(f) = f()

        member this.While(guard, body) =
            if not (guard()) 
            then this.Zero() 
            else this.Bind( body(), fun () -> 
                this.While(guard, body))  

        member this.TryWith(body, handler) =
            try this.ReturnFrom(body())
            with e -> handler e

        member this.TryFinally(body, compensation) =
            try this.ReturnFrom(body())
            finally compensation() 

        member this.Using(disposable:#System.IDisposable, body) =
            let body' = fun () -> body disposable
            this.TryFinally(body', fun () -> 
                match disposable with 
                    | null -> () 
                    | disp -> disp.Dispose())

        member this.For(sequence:seq<_>, body) =
            this.Using(sequence.GetEnumerator(),fun enum -> 
                this.While(enum.MoveNext, 
                    this.Delay(fun () -> body enum.Current)))

        member this.Combine (a,b) = 
            this.Bind(a, fun () -> b())

    let asyncResult = AsyncResultBuilder()

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

type Lambda<'R> =
        abstract Invoke<'T> : 'T -> 'R

type Envelope =
    abstract Address: Address
and Envelope<'message> = { 
        Message : 'message
        Address: Address }
with interface Envelope with
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

    type Behavior<'state, 'message> = ^state -> 'message -> 'state

    type Create< 'state, 'message> = Behavior< 'state, 'message> -> 'state -> Address

    let inline unpack (envelope: Envelope) = envelope :?> Envelope<'message>

    let inline create (BoundedContext context): Create< 's, 'm> = fun (behavior: Behavior< 's, 'm>) initialState ->
            let agent: MailboxProcessor<Envelope> = 
                    MailboxProcessor.Start(fun inbox ->
                        let rec messageLoop state = async {
                            let! envelope = inbox.Receive()

                            let envelope' = envelope |> unpack
                        
                            let newState = behavior state envelope'.Message

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


module BoundedContext =

    open Actor

    let create (resolve : Resolve) (forward: Forward) =

        let registry = Map.empty

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

        send, context


module IO = 

    open TrustBoundary

    type PayloadAccepted<'payload> = Event<'payload>

    type PayloadDeclined<'payload> = Event<'payload>

    type Accept<'payload, 'message> = Input<'payload, 'message> -> BoundedContext -> Result<PayloadAccepted<'payload>, PayloadDeclined<'payload>>

    type Publish<'payload, 'message> = Output<'message, 'payload> -> Envelope<'message> -> unit
