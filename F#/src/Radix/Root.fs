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

type Version = Version of Int64

type Command<'command> = { 
        Payload : 'command
        OriginalVersion: Version
        }

type Hash = Hash of byte[]

[<AbstractClass>]
type Address<'command>(guid: Guid) = 
    member private this.Hash = 
        let sha1 = SHA1.Create()
        Hash (sha1.ComputeHash(guid.ToByteArray()))
    
    interface IComparable<Address<'command>> with
        member x.CompareTo(y) = 
            let (Hash x) = x.Hash
            let (Hash y) = y.Hash
            BitConverter.ToString(x).CompareTo(BitConverter.ToString(y))

    interface IComparable with 
        member x.CompareTo(y) = 
            let (Hash x) = x.Hash
            let address: Address<'command> = (downcast y) 
            let (Hash y) = address.Hash
            BitConverter.ToString(x).CompareTo(BitConverter.ToString(y))

    abstract member Accept : 'command * Version -> unit

    

type ValidationError = Undefined

type Validated<'t> = Validated of 't

type Validate<'t> = 't -> Result<Validated<'t>, ValidationError>

type Map<'a, 'b> = Validated<'a> -> 'b

type Lambda<'R> =
        abstract Invoke<'T> : 'T -> 'R






type DTO<'payload> = {
    Address : Address<'payload>
    Payload : 'payload
    }

module TrustBoundary =

    type Deserialize<'payload> = Stream -> DTO<'payload>

    type Listen = unit -> Stream

    // type Pack<'message> = Envelope<'message> -> Envelope

    type Input<'payload, 'message> = Deserialize<'payload> -> Validate<'payload> -> Map<'payload, 'message> ->  Result<Command<'message>, ValidationError>

    type Serialize<'message> = Command<'message> -> MemoryStream -> Stream

    type Output<'message, 'payload> = Command<'message> -> DTO<'payload> -> Serialize<'message>

open TrustBoundary

module Routing =

    type AddressNotFoundError = AddressNotFoundError of string

    type Resolve<'message> = Address<'message> -> AsyncResult<Uri, AddressNotFoundError>

    type CommandPosted<'message> = Event<Command<'message>>

    type CommandForwarded<'message> = Event<Command<'message>>

    type Agent<'command> = Agent of MailboxProcessor<'command * Version * Address<'command>>

    type Registry<'message> = Registry of FSharp.Collections.Map<Address<'message>, Agent<'message>>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type Forward<'command> = Uri -> 'command -> AsyncResult<CommandForwarded<'command>, UnableToDeliverEnvelopeError>

open Routing

type RegisterAgentCommand<'message> = {
        Agent: Agent<'message>
    }

type SaveEventsCommand<'command, 'event> = {
    Address: Address<'command>
    Events: 'event list
    ExpectedVersion: Version
}
    
type AgentRegistered<'message> = {
        Address: Address<'message>
    }
    
type ContextCommand<'command, 'event> =
    | IssueCommand of 'command * Version * Address<'command>
    | SaveEventsCommand of SaveEventsCommand<'command, 'event>
    | RegisterAgent of RegisterAgentCommand<'command> * AsyncReplyChannel<AgentRegistered<'command>> 


type Aggregate<'state, 'command, 'event> = Address<'command> -> ^state -> 'command -> 'state * 'event list

type Create< 'state, 'command, 'event> = Aggregate< 'state, 'command, 'event> -> 'state -> Address<'command>

type SaveEvents<'command, 'event> = Address<'command> -> 'event list -> Version -> unit
    
type BoundedContext<'command, 'event> = BoundedContext of MailboxProcessor<ContextCommand<'command, 'event>>
    with
        member inline this.create< ^state, ^event>  (behavior: Aggregate< 'state, 'command, 'event>) initialState =

            let (BoundedContext context) = this

            let agent: MailboxProcessor<'command * Version * Address<'command>> = 
                    MailboxProcessor.Start(fun inbox ->
                        let rec messageLoop state = async {
                            let! (command, version, address) = inbox.Receive()
                        
                            let (newState, events) = behavior address state command

                            let saveEventsCommand : SaveEventsCommand<'command, 'event> = {
                                Address = address
                                Events = events
                                ExpectedVersion = version
                            }

                            context.Post (SaveEventsCommand saveEventsCommand)

                            return! messageLoop newState
                        }

                        messageLoop initialState
                    )   
            let registerAgentCommand: RegisterAgentCommand<'command> = {
                Agent = Agent agent
            }
            
            let agentRegistered = 
                context.PostAndAsyncReply (fun channel ->  RegisterAgent (registerAgentCommand, channel))
                |> Async.RunSynchronously


            agentRegistered.Address

module Actor = 

    let inline (+=) (context: BoundedContext<'command, 'event> ) (behavior, initialState) = 
        context.create behavior initialState

    let inline (<--) (address: Address< ^command>) (command: ^command, version: Version) = address.Accept (command, version)



module BoundedContext =

    open Actor

    let create (saveEvents : SaveEvents<'command, 'event>) (resolve : Resolve<'command>) (forward: Forward<'command>) =

        let registry = Map.empty

        let context = BoundedContext (MailboxProcessor.Start(fun inbox -> 
                let rec messageLoop (Registry state) = async {
                    let! command = inbox.Receive()
                    match command with
                    | IssueCommand (command, version, address) ->
                        match state.TryFind address with
                        | Some (Agent agent) -> 
                            agent.Post (command, version, address)
                        | _ -> 
                            let! resolveResult = resolve address
                            match resolveResult with
                            | Ok uri-> 
                                let! forwardResult = forward uri command
                                forwardResult |> ignore // log
                                return! messageLoop (Registry state)
                            | Error error -> 
                                error |> ignore //log
                                return! messageLoop (Registry state)

                     | RegisterAgent (command, replyChannel) ->

                        let guid = Guid.NewGuid()                        

                        let address = 
                            {  new Address<'command>(guid) with            
                                member this.Accept (command, version) = 
                                    inbox.Post (IssueCommand (command, version, this))}
                        let newState = state.Add (address, command.Agent)
   
                        let agentRegistered: AgentRegistered<'command> = {
                                 Address = address
                        }
                        replyChannel.Reply agentRegistered
                        return! messageLoop (Registry newState)

                    | SaveEventsCommand saveEventsCommand ->
                        saveEvents saveEventsCommand.Address saveEventsCommand.Events saveEventsCommand.ExpectedVersion

                    return! messageLoop (Registry state)
                }
                    
                messageLoop (Registry registry)
                    
            )
        )

        context


module IO = 

    open TrustBoundary

    type PayloadAccepted<'payload> = Event<'payload>

    type PayloadDeclined<'payload> = Event<'payload>

    //type Accept<'payload, 'message> = Input<'payload, 'message> -> BoundedContext<'message> -> Result<PayloadAccepted<'payload>, PayloadDeclined<'payload>>

    type Publish<'payload, 'message> = Output<'message, 'payload> -> Command<'message> -> unit
