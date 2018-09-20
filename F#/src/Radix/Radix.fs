namespace Radix

open System.Security.Principal
open System
open System.IO
open System.Security.Cryptography

type Undefined = exn

type Hash = Hash of byte[]

[<AbstractClass>]
type Address<'message>(guid: Guid) = 
    member private this.Hash = 
        let sha1 = SHA1.Create()
        Hash (sha1.ComputeHash(guid.ToByteArray()))
    
    interface IComparable<Address<'message>> with
        member x.CompareTo(y) = 
            let (Hash x) = x.Hash
            let (Hash y) = y.Hash
            BitConverter.ToString(x).CompareTo(BitConverter.ToString(y))

    interface IComparable with 
        member x.CompareTo(y) = 
            let (Hash x) = x.Hash
            let address: Address<'message> = (downcast y) 
            let (Hash y) = address.Hash
            BitConverter.ToString(x).CompareTo(BitConverter.ToString(y))

    abstract member Send : 'message -> unit 

type Payload<'message> =
    | Message of 'message
    | Stream of Stream

type Agent<'message> = Agent of MailboxProcessor<'message>

type Registry<'message> = Registry of Map<Address<'message>, Agent<'message>>

type Command<'message, 'response> = {
    Origin: Address<'response>
    Payload: 'message
    Version: Version
    Principal: IPrincipal
    Timestamp: DateTimeOffset
}

type Event<'message> = {
    Payload: 'message
    Timestamp: DateTimeOffset
}

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