module CellProperties

open FsCheck.Xunit
open Root
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks


type CellMessage< ^a> = 
| Set of ^a
| Get of Address<CellMessage<'a>>
| Reply of ^a

let inline serialize message (stream: Stream) =
    
    let writer = new StreamWriter(stream)
    let jsonWriter = new JsonTextWriter(writer)
    let ser = new JsonSerializer();
    ser.Serialize(jsonWriter, message);
    jsonWriter.Flush();
    stream

let inline deserialize (stream: Stream) =
    stream.Seek (0L, SeekOrigin.Begin) |> ignore
    use reader = new StreamReader(stream)
    use jsonReader = new JsonTextReader(reader)
    let ser = new JsonSerializer();
    let m = ser.Deserialize<'message>(jsonReader);
    m
    

let inline forward _ __ =
    AsyncResult.ofError (Root.Routing.UnableToDeliverEnvelopeError "")

let inline resolveRemoteAddress _ =
    AsyncResult.ofError (Root.Routing.AddressNotFoundError "")

let currentEvents = []

let saveEvents: SaveEvents<'command, unit> = fun _ events _ ->
    currentEvents |> List.append events |> ignore

let context = BoundedContext.create saveEvents resolveRemoteAddress forward

open Actor

let cellBehavior : Aggregate<'state, 'command, 'event> = fun _ state command ->
    match command with
    | Get customer -> 
        customer <-- (Reply state, Version 1L) 
        
        state, []
    | Set value -> 
        value, []
    | _ -> state, []

let ignoreBehavior : Aggregate<'state, 'command, 'event> = fun _ state _ -> state, []

let inline exposeBehavior (taskCompletionSource: TaskCompletionSource< ^state * ^message>) = fun _ state message ->
    taskCompletionSource.SetResult(state, message)
    state, []

[<Property(Verbose = true)>]
let ``Getting the value of a cell returns the expected value`` (value: int) =
    let taskCompletionSource = new TaskCompletionSource<'state * 'message>()

    let cell = context += (cellBehavior, 1)
    let customer = context += ((exposeBehavior taskCompletionSource), 0)
    
    cell <-- (Set value, Version 1L)
    cell <-- (Get customer, Version 1L)

    let t = taskCompletionSource.Task |> Async.AwaitTask |> Async.RunSynchronously
    match t with
    | (_, Reply reply) -> 
        Assert.Equal(value, reply)
    | _ -> Assert.True false

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
    
