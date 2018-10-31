module AggregateProperties

open FsCheck.Xunit
open Root
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks
open System

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

open Root

type DeactivateInventoryItem = unit

type CreateInventoryItem = CreateInventoryItem of string

type RenameInventoryItem = RenameInventoryItem of string

type CheckInItemsToInventory = CheckInItemsToInventory of int

type RemoveItemsFromInventory = RemoveItemsFromInventory of int

type InventoryCommand = 
    | DeactivateInventoryItem of unit
    | CreateInventoryItem of string
    | RenameInventoryItem of string
    | CheckInItemsToInventory of int
    | RemoveItemsFromInventory of int

type InventoryItemDeactivated = {
    Address: Address<InventoryCommand>
}

type InventoryItem = {
    Name: string
    Activated: bool
    Count: int
}

type InventoryEvent =
    | InventoryItemCreated of InventoryItem
    | InventoryItemDeactivated of unit
    | InventoryItemRenamed of string
    | ItemsCheckedInToInventory of int
    | ItemsRemovedFromInventory of int

module Domain = 

    open Microsoft.FSharp.Data.UnitSystems.SI

    [<Measure>] type ms

    type Timeout = Timeout of float<ms>

    type SetCommand<'message> = {
        Message: 'message
        Customer: Address<'message>
        Timeout: Timeout
    }

    type Set<'message> = Set of SetCommand<'message>

    let inline alarmClockBehavior _ message =
        match message with
        | Set m ->
            let (Timeout timeout) = m.Timeout
            let timer = new System.Timers.Timer(timeout/1.0<ms>)
            let sendEventHandler _ = m.Customer <-- m.Message
            timer.Elapsed.Add (sendEventHandler)
            timer.Enabled = true        


    let inventoryItemAggregate: Aggregate<InventoryItem, InventoryCommand, InventoryEvent> = fun address state command -> 
           match command with       
           | DeactivateInventoryItem _ ->

                let newState = { state with Activated = false }
     
                newState, [InventoryItemDeactivated ()]

           | RenameInventoryItem name ->

                let newState = { state with Name = name }

                newState, [InventoryItemRenamed name]
      
            | CheckInItemsToInventory count ->
                match count > 0 with
                | true -> 
                    let newState = { state with Count = state.Count + count }
                    newState,  [ItemsCheckedInToInventory count]
                | false -> state, []               

            | RemoveItemsFromInventory count ->
                match count > 0 with
                | true -> 
                    let newState = { state with Count = state.Count - count }
              
                    newState, [ItemsRemovedFromInventory count]
                | false -> state, []   
             
            | _ -> state, []   

open Actor
open Domain

let currentEvents = []

let saveEvents: SaveEvents<InventoryCommand, InventoryEvent> = fun _ events _ ->
    currentEvents |> List.append events |> ignore

[<Property(Verbose = true)>]
let ``foo`` (value: int) =
    let context = BoundedContext.create saveEvents resolveRemoteAddress forward

    let inventoryItem = context += (inventoryItemAggregate, {
        Name = "Product 1"
        Activated = true
        Count = 0
    })

    inventoryItem <-- (CheckInItemsToInventory 2, Version 1L)