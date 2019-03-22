module AggregateProperties

open FsCheck.Xunit
open Root

let inline forward _ __ =
    AsyncResult.ofError (Root.Routing.UnableToDeliverEnvelopeError "")

let inline resolveRemoteAddress _ =
    AsyncResult.ofError (Root.Routing.AddressNotFoundError "")

open Root

type InventoryCommand = 
    | DeactivateInventoryItem of unit
    | CreateInventoryItem of string
    | RenameInventoryItem of string
    | CheckInItemsToInventory of int
    | RemoveItemsFromInventory of int

type InventoryEvent =
    | InventoryItemCreated of string
    | InventoryItemDeactivated of unit
    | InventoryItemRenamed of string
    | ItemsCheckedInToInventory of int
    | ItemsRemovedFromInventory of int

type InventoryItem = {
    Name: string
    Activated: bool
    Count: int
}
with
    static member inline Zero = {
        Name = ""
        Count = 0
        Activated = false
    }
    static member inline decide (_, state, command) = 
        match command with       
            | DeactivateInventoryItem _ -> [InventoryItemDeactivated ()]
            | RenameInventoryItem name -> [InventoryItemRenamed name]
            | CheckInItemsToInventory count ->
                match count > 0 with
                | true -> [ItemsCheckedInToInventory (state.Count + count)]
                | false -> []               
            | RemoveItemsFromInventory count ->
                match count > 0 with
                | true -> [ItemsRemovedFromInventory (state.Count - count)]
                | false -> []   
            | CreateInventoryItem name -> [InventoryItemCreated name]
    static member inline apply (state, event) = 
        match event with
        | InventoryItemCreated name -> { state with Name = name }
        | InventoryItemDeactivated _ -> { state with Activated = false }
        | ItemsCheckedInToInventory newCount -> { state with Count = newCount }
        | ItemsRemovedFromInventory newCount -> { state with Count = newCount }
        | InventoryItemRenamed name -> { state with Name = name }

module Domain = 

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

open Domain
open Operators
open Aggregate

let currentEvents = []

let saveEvents: SaveEvents<'command, 'event> = fun _ events _ ->
    currentEvents |> List.append events

[<Property(Verbose = true)>]
let ``foo`` (value: int) =
    let context = BoundedContext.create saveEvents resolveRemoteAddress forward

    let history = [InventoryItemCreated "Product 1"]

    let inventoryItem = Aggregate.create<InventoryItem, InventoryCommand, InventoryEvent> context history

    inventoryItem <-- (CheckInItemsToInventory value, Version 1L)

//open Radix.Math.Applied.Learning.Reinforced.Testing.OpenAI.Gym.Api
//open System
//[<Property(Verbose = true)>]
//let ``bar`` (value: int) =

//    let baseUrl = "http://127.0.0.1:5000"

//    let environmentId = "FrozenLake-v0"

//    let environment = Environment.create baseUrl environmentId
//    ActionSpace.get baseUrl environment.InstanceId

//    ObservationSpace.get baseUrl environment.InstanceId

//    Monitor.start baseUrl environment.InstanceId "frozenlake" true false false

//    Environment.reset baseUrl environment.InstanceId
//    Environment.step baseUrl environment.InstanceId 3 true
//    Environment.step baseUrl environment.InstanceId 3 true
//    Environment.step baseUrl environment.InstanceId 0 true
//    Environment.step baseUrl environment.InstanceId 0 true
//    Environment.step baseUrl environment.InstanceId 1 true
//    Environment.step baseUrl environment.InstanceId 1 true
//    Environment.step baseUrl environment.InstanceId 2 true
//    //Environment.step baseUrl environment.InstanceId 2 true

//    Monitor.stop baseUrl environment.InstanceId
