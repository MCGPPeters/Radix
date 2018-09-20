module CellProperties

open FsCheck.Xunit
open Radix.Primitives
open Radix
open Radix.Routing.Types
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks
open System.Threading

type Set<'a> = Set of 'a

type CellMessage<'a> = 
| Set of 'a
| Get of Address<CellMessage<'a>>
| Reply of 'a

let serialize: Serialize<'message> = fun message stream ->
    
    let writer = new StreamWriter(stream)
    let jsonWriter = new JsonTextWriter(writer)
    let ser = new JsonSerializer();
    ser.Serialize(jsonWriter, message);
    jsonWriter.Flush();
    stream

let deserialize : Deserialize<'message> = fun stream ->
    stream.Seek (int64(0), SeekOrigin.Begin) |> ignore
    use reader = new StreamReader(stream)
    use jsonReader = new JsonTextReader(reader)
    let ser = new JsonSerializer();
    let m = ser.Deserialize<'message>(jsonReader);
    m
    

let forward: Forward<'message> = fun _ __ ->
    AsyncResult.ofError (UnableToDeliverEnvelopeError "")

let resolveRemoteAddress: ResolveRemoteAddress<'message> = fun _ ->
    AsyncResult.ofError (AddressNotFoundError "")

let primitives = Node.create deserialize serialize resolveRemoteAddress forward

let cellBehavior = fun state message ->
    match message with
    | Get customer -> 
        customer <-- (Reply state)
        state
    | Set value -> 
        value

let exposeBehavior (taskCompletionSource: TaskCompletionSource<'state *' message>) : Behavior<'state, 'message> = fun state message ->
    taskCompletionSource.SetResult(state, message)
    state



[<Property(Verbose = true)>]
let ``Getting the value of a cell returns the expected value`` (value: int) =
    let taskCompletionSource = new TaskCompletionSource<'state * 'message>()

    let cell = primitives.Create cellBehavior 1
    let customer = primitives.Create (exposeBehavior taskCompletionSource) 0
    
    cell <-- (Set value)
    cell <-- (Get customer)

    let (_, (Reply reply)) = taskCompletionSource.Task |> Async.AwaitTask |> Async.RunSynchronously
    Assert.Equal(value, reply)

[<Property(Verbose = true)>]
let ``Getting the value of a cell returns the expected value when message is received as stream`` (value: int) =
    let taskCompletionSource = new TaskCompletionSource<'state * 'message>()

    let cell = primitives.Create cellBehavior 1
    let customer = primitives.Create (exposeBehavior taskCompletionSource) 0
    
    let set = serialize (Set value) (new MemoryStream())
    let envelope = {
        Destination = cell
        Principal = Thread.CurrentPrincipal
        Payload = (Radix.Stream set)
        }

    primitives.Receive envelope
    cell <-- (Get customer)

    let (_, (Reply reply)) = taskCompletionSource.Task |> Async.AwaitTask |> Async.RunSynchronously
    Assert.Equal(value, reply)
    
