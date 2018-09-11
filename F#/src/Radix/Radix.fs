namespace Radix

open System.Security.Principal
open System
open System.IO
open System.Security.Cryptography

type Undefined = exn

type Hash = Hash of byte[]

type Address = private Address of Hash

module Address = 

    let create = 
        let sha1 = SHA1.Create();
        Address (Hash (sha1.ComputeHash(Guid.NewGuid().ToByteArray())))

    let value (Address address) = address

type Command<'a> = {
    Payload: 'a
    Principal: IPrincipal
    Timestamp: DateTimeOffset
}

type Event<'a> = {
    Aggregate: Address
    Payload: 'a
    Timestamp: DateTimeOffset
}

type Agent = MailboxProcessor<Stream>

type Actor = {
    Address: Address
    Agent: Agent
}

type Registry = Registry of Map<Address, Agent>


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