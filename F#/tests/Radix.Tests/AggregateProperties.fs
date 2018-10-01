module AggregateProperties

open FsCheck.Xunit
open Radix.Primitives
open Radix
open Radix.Routing.Types
open System.IO
open Newtonsoft.Json
open Xunit
open System.Threading.Tasks
open System.Threading
open System

type Aggregate<'aggregate> = {
    Id: Guid
    Aggregate: 'aggregate
    Version: Version
}

type InventoryItem = {
    Name: string
    Activated: bool
}

type InventoryItemAggregate = Aggregate<InventoryItem>

type InventoryItemCreated = {
    InventoryItem: InventoryItem
}

type InventoryItemDeactivated = {
    Id: Guid
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

type InventoryEvent =
    //| InventoryItemCreated of Event<InventoryItem>
    | InventoryItemDeactivated of Event<InventoryItemDeactivated>
    | InventoryItemRenamed of Event<InventoryItemRenamed>
    | ItemsCheckedInToInventory of Event<ItemsCheckedInToInventory>
    | ItemsRemovedFromInventory of Event<ItemsRemovedFromInventory>

type DeactivateInventoryItem = {
    Id: Guid
    OriginalVersion: int
}

type CreateInventoryItem = {
    Id: Guid
    Name: string
}

type RenameInventoryItem = {
    Id: Guid
    OriginalVersion: int
    Name: string
}

type CheckInItemsToInventory = {
    Id: Guid
    OriginalVersion: int
    Count: int
}

type RemoveItemsFromInventory = {
    Id: Guid
    OriginalVersion: int
    Count: int
}

type InventoryCommand = 
    | DeactivateInventoryItem of Command<DeactivateInventoryItem, Event<InventoryItemDeactivated>>
    //| CreateInventoryItem of Command<CreateInventoryItem, Event<InventoryItemCreated>>
    | RenameInventoryItem of Command<RenameInventoryItem, Event<InventoryItemRenamed>>
    | CheckInItemsToInventory of Command<CheckInItemsToInventory, Event<ItemsCheckedInToInventory>>
    | RemoveItemsFromInventory of Command<RemoveItemsFromInventory, Event<ItemsRemovedFromInventory>>



let inline createInventoryItemBehavior (storeEvent : InventoryEvent -> unit) = fun (state: InventoryItemAggregate, message) ->
    match message with       
    | DeactivateInventoryItem deactivateInventoryItem ->
        
        match state.Version.Equals(deactivateInventoryItem.Version) with 
        | true ->
            let aggregateState = { state.Aggregate with Activated = false }
            let newState = { state with Aggregate = aggregateState }
            let inventoryItemDeactivated : Event<InventoryItemDeactivated> = {
                    Payload = { Id = deactivateInventoryItem.Payload.Id }
                    Timestamp = DateTimeOffset.Now
                }
            storeEvent (InventoryItemDeactivated inventoryItemDeactivated)
            deactivateInventoryItem.Origin <-- inventoryItemDeactivated
            Ok newState
        | _ -> Error "Concurrency error"

    | RenameInventoryItem renameInventoryItem ->

        match state.Version.Equals(renameInventoryItem.Version) with 
        | true ->
            let aggregateState = { state.Aggregate with Name = renameInventoryItem.Payload.Name }
            let newState = { state with Aggregate = aggregateState }
            let inventoryItemRenamed : Event<InventoryItemRenamed> = {
                    Payload = { 
                        Id = renameInventoryItem.Payload.Id 
                        Name = renameInventoryItem.Payload.Name}
                    Timestamp = DateTimeOffset.Now
                }
            storeEvent (InventoryItemRenamed inventoryItemRenamed)
            renameInventoryItem.Origin <-- inventoryItemRenamed
            Ok newState
        | _ -> Error "Concurrency error"
    //| CheckInItemsToInventory checkInItemsToInventory
    //| RemoveItemsFromInventory removeItemsFromInventory

//[<Property(Verbose = true)>]
//let ```` (value: int) =