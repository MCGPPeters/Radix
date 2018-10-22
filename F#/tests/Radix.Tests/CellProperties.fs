module CellProperties

open FsCheck.Xunit
open Root
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks

type Set< ^a> = Set of ^a

type CellMessage< ^a> = 
| Set of ^a
| Get of Address
| Reply of ^a

let inline serialize message (stream: Stream) =
    
    let writer = new StreamWriter(stream)
    let jsonWriter = new JsonTextWriter(writer)
    let ser = new JsonSerializer();
    ser.Serialize(jsonWriter, message);
    jsonWriter.Flush();
    stream

let inline deserialize (stream: Stream) =
    stream.Seek (int64(0), SeekOrigin.Begin) |> ignore
    use reader = new StreamReader(stream)
    use jsonReader = new JsonTextReader(reader)
    let ser = new JsonSerializer();
    let m = ser.Deserialize<'message>(jsonReader);
    m
    

let inline forward _ __ =
    AsyncResult.ofError (Root.Routing.UnableToDeliverEnvelopeError "")

let inline resolveRemoteAddress _ =
    AsyncResult.ofError (Root.Routing.AddressNotFoundError "")

let primitives = BoundedContext.create resolveRemoteAddress forward

let (<--) address message = 
    let send = primitives |> fst
    send address message

let (*) behavior = 
    let context = primitives |> snd
    Actor.create context behavior

let cellBehavior : Actor.Behavior<'state, 'message> = fun state message ->
    match message with
    | Get customer -> 
        customer <-- message 
        
        state
    | Set value -> 
        value

let ignoreBehavior : Actor.Behavior<'state, 'message> = fun state message -> state

let inline exposeBehavior (taskCompletionSource: TaskCompletionSource< ^state * ^message>) = fun state message ->
    taskCompletionSource.SetResult(state, message)
    state



[<Property(Verbose = true)>]
let ``Getting the value of a cell returns the expected value`` (value: int) =
    let taskCompletionSource = new TaskCompletionSource<'state * 'message>()

    let cell = cellBehavior * 1
    let customer = exposeBehavior taskCompletionSource * 0
    
    cell <-- (Set value)
    cell <-- (Get customer)

    let (_, (Reply reply)) = taskCompletionSource.Task |> Async.AwaitTask |> Async.RunSynchronously
    Assert.Equal(value, reply)

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
    
