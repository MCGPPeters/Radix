module CellProperties

open FsCheck.Xunit
open Root
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks

type CellCommand<'state> = 
    | Set of 'state
    | Get of Address<CellCommand<'state>>
    | Reply of 'state


type CellEvent<'state> =
    | StateSet of 'state

type Cell<'state> = Cell of 'state option
with
    static member inline Zero: 'state option = None
    static member inline decide (state, command) = 
        match command with       
            | Set state -> [StateSet state]
            | Get address -> 
                address <-- state
                []
            | Reply _ -> [] 
    static member inline apply (_, event) = 
        match event with
        | StateSet state' -> state'
   

let inline forward _ __ =
    AsyncResult.ofError (Root.Routing.UnableToDeliverEnvelopeError "")

let inline resolveRemoteAddress _ =
    AsyncResult.ofError (Root.Routing.AddressNotFoundError "")

let currentEvents = []

let saveEvents: SaveEvents<'command, 'event> = fun _ events _ ->
    currentEvents |> List.append events

let context = BoundedContext.create saveEvents resolveRemoteAddress forward

open Operators

//[<Property(Verbose = true)>]
//let ``Getting the value of a cell returns the expected value`` (value: int) =
//    let context = BoundedContext.create saveEvents resolveRemoteAddress forward

//    let cell = Aggregate.create context [StateSet (Some 1)]
//    let empty = Aggregate.create context [StateSet (Some 0)]
    
//    cell <-- (Set value, Version 1L)
//    cell <-- (Get empty, Version 1L)


//[<Property(Verbose = true)>]
//let ``Getting the value of a cell returns the expected value when message is received as stream`` (value: int) =
//    let taskCompletionSource = new TaskCompletionSource<'state * 'message>()

//    let cell = cellBehavior * 1
//    let customer = exposeBehavior taskCompletionSource * 0
    
//    let set = serialize (Set value) (new MemoryStream())
//    let envelope = {
//        Destination = cell
//        Principal = Thread.CurrentPrincipal
//        Payload = (Radix.Stream set)
//        }

//    primitives.Receive envelope
//    primitives.Send cell (Get customer)

//    let (_, (Reply reply)) = taskCompletionSource.Task |> Async.AwaitTask |> Async.RunSynchronously
//    Assert.Equal(value, reply)
    
