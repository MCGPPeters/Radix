module Tests

open FsCheck.Xunit
open Radix.Primitives
open Radix
open Radix.Routing.Types
open System.IO
open Newtonsoft.Json
open Xunit

type Set<'a> = Set of 'a

type CellMessage<'a> = 
| Set of 'a
| Get of Address
| Reply of 'a

let serialize: Serialize<'message> = fun message stream ->
    
    let writer = new StreamWriter(stream)
    let jsonWriter = new JsonTextWriter(writer)
    let ser = new JsonSerializer();
    ser.Serialize(jsonWriter, message);
    jsonWriter.Flush();

let deserialize : Deserialize<'message> = fun stream ->
    stream.Seek (int64(0), SeekOrigin.Begin);
    use reader = new StreamReader(stream)
    use jsonReader = new JsonTextReader(reader)
    let ser = new JsonSerializer();
    let m = ser.Deserialize<'message>(jsonReader);
    m
    

let forward: Forward = fun _ __ ->
    AsyncResult.ofError (UnableToDeliverEnvelopeError "")

let registry = Registry (Map.empty)

let resolveRemoteAddress: ResolveRemoteAddress = fun _ ->
    AsyncResult.ofError (AddressNotFoundError "")

let primitives = Node.create registry deserialize serialize resolveRemoteAddress forward

let cellBehavior = fun state message ->
    match message with
    | Get customer -> 
        primitives.Send customer (Reply state)
        state
    | Set value -> 
        value

[<Property>]
let ``Getting the value of a cell returns the expected value`` (value: int) =
    let cell = primitives.Create cellBehavior 1
    let customer = primitives.Create (fun state message ->
            match message with
            | Reply value -> 
                Assert.Equal(2, value)
                value
            | _ -> 
                Assert.False(true)
                state) 0
    primitives.Send cell (Set value)
    primitives.Send cell (Get customer)
    
