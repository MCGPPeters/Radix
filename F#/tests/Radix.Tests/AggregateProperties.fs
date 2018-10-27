open System
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



type InventoryItemDeactivated = {
    Id: Guid
}

type Aggregate<'aggregate> = {
    Id: Guid
    Aggregate: 'aggregate
    Version: Version
}

type InventoryItem = {
    Name: string
    Activated: bool
    Count: int
}



type InventoryItemAggregate = Aggregate<InventoryItem>

type InventoryItemCreated = {
    InventoryItem: InventoryItem
}



type InventoryItemRenamed = {
    Id: Guid
    Name: string
}

type ItemsCheckedInToInventory = {
    Id: Guid
    Count: int
}

type ItemsRemovedFromInventory = {
    Id: Guid
    Count: int
}

type Command<'command> = Command of 'command
type Event<'event> = Event of 'event

type Version = Version of int64

type InventoryEvent =
    | InventoryItemCreated of Event<InventoryItem>
    | InventoryItemDeactivated of Event<InventoryItemDeactivated>
    | InventoryItemRenamed of Event<InventoryItemRenamed>
    | ItemsCheckedInToInventory of Event<ItemsCheckedInToInventory>
    | ItemsRemovedFromInventory of Event<ItemsRemovedFromInventory>

type DeactivateInventoryItem = {
    Id: Guid
    OriginalVersion: Version
}

type CreateInventoryItem = {
    Id: Guid
    Name: string
}

type RenameInventoryItem = {
    Id: Guid
    OriginalVersion: Version
    Name: string
}

type CheckInItemsToInventory = {
    Id: Guid
    OriginalVersion: Version
    Count: int
}

type RemoveItemsFromInventory = {
    Id: Guid
    OriginalVersion: Version
    Count: int
}

type InventoryCommand = 
    | DeactivateInventoryItem of Command<DeactivateInventoryItem>
    | CreateInventoryItem of Command<CreateInventoryItem>
    | RenameInventoryItem of Command<RenameInventoryItem>
    | CheckInItemsToInventory of Command<CheckInItemsToInventory>
    | RemoveItemsFromInventory of Command<RemoveItemsFromInventory>


module Domain = 
    let inline createInventoryItemBehavior (storeEvent : InventoryEvent -> unit) = fun (state: InventoryItemAggregate, message) ->
       match message with       
       | DeactivateInventoryItem (Command deactivateInventoryItem) ->
            
           match state.Version.Equals(deactivateInventoryItem.OriginalVersion) with 
           | true ->
               let aggregateState = { state.Aggregate with Activated = false }
               let newState = { state with Aggregate = aggregateState }
               let inventoryItemDeactivated : Event<InventoryItemDeactivated> = Event {
                                                    Id = deactivateInventoryItem.Id }

                   
               storeEvent (InventoryItemDeactivated inventoryItemDeactivated)
               Ok newState
           | _ -> Error "Concurrency error"

       | RenameInventoryItem (Command renameInventoryItem) ->

           match state.Version.Equals(renameInventoryItem.OriginalVersion) with 
           | true ->
               let aggregateState = { state.Aggregate with Name = renameInventoryItem.Name }
               let newState = { state with Aggregate = aggregateState }
               let inventoryItemRenamed : Event<InventoryItemRenamed> = Event {
                                               Id = renameInventoryItem.Id 
                                               Name = renameInventoryItem.Name}

               storeEvent (InventoryItemRenamed inventoryItemRenamed)
               Ok newState
           | _ -> Error "Concurrency error"//let inline createInventoryItemBehavior (storeEvent : InventoryEvent -> unit) = fun (state: InventoryItemAggregate, message) ->
      
        | CheckInItemsToInventory (Command checkInItemsToInventory) ->
            match state.Version.Equals(checkInItemsToInventory.OriginalVersion) with
            | true -> match checkInItemsToInventory.Count > 0 with
                        | true -> 
                            let aggregateState = { state.Aggregate with Count = state.Aggregate.Count + checkInItemsToInventory.Count }
                            let newState = { state with Aggregate = aggregateState }
                            let itemsCheckedInToInventory : Event<ItemsCheckedInToInventory> = Event {
                                    Id = checkInItemsToInventory.Id 
                                    Count = checkInItemsToInventory.Count}
                            storeEvent (ItemsCheckedInToInventory itemsCheckedInToInventory)
                            Ok newState
                        | false -> Error "Must have a count greater than 0 to add to inventory"               
            | false ->  Error "Concurrency error"

        | RemoveItemsFromInventory (Command removeItemsFromInventory) ->
           match state.Version.Equals(removeItemsFromInventory.OriginalVersion) with 
           | true -> match removeItemsFromInventory.Count > 0 with
                        | true -> 
                            let aggregateState = { state.Aggregate with Count = state.Aggregate.Count - removeItemsFromInventory.Count }
                            let newState = { state with Aggregate = aggregateState }
                            let itemsRemovedFromInventory : Event<ItemsRemovedFromInventory> = Event {
                                    Id = removeItemsFromInventory.Id 
                                    Count = removeItemsFromInventory.Count}
                            storeEvent (ItemsRemovedFromInventory itemsRemovedFromInventory)
                            Ok newState
                        | false -> Error "Can not remove negative count from inventory"
           | false -> Error "Concurrency error"              
        | _ -> Error "Unknown message" 

open Actor
open Domain

[<Property(Verbose = true)>]
let ``foo`` (value: int) =
    let context = BoundedContext.create resolveRemoteAddress forward
    let inventoryItemBehavior = createInventoryItemBehavior (fun (InventoryEvent event) -> {
        match event with
        | DeactivateInventoryItem deactivate -> ignore
        | RemoveItemsFromInventory remove -> ignore
        | CheckInItemsToInventory checkin -> ignore
        | RenameInventoryItem rename -> ignore
    })

    let inventoryItem = context += inventoryItemBehavior {
        Name = "Product 1"
        Activated = true
        Count = 0
    }

    let checkin = {
        Id = Guid.NewGuid()
        OriginalVersion = 0
        Count = 2
    }

    inventoryItem <-- checkin