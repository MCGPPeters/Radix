# Aggregate migration

An instance of a bounded context can on multiple nodes simultaneously. An aggregate however can only live in 1 node at a time. It can however migrate to another node when certain criteria are met. This can be done pretty easily. Since all events are stored centrally, the aggregate can be created on the other node and restore its state there. The binaries containing the logic for the context also contains the logic for the aggregates it contains.

# Command and event hierarchies

Creating a hierarchy of events and commands helps determining the scope of those. At the top level of the command hierarchy is the overarching type of the bounded context. For instance withing the bounded context of inventory management it could be InventoryCommand. The same goes for events : InventoryEvent. The second level of the hierarchy will be at the aggregate root level. Think of InventoryItemCommand and InventoryItemEvent. The easiest way of implementing these hierarchies is using sum types (discriminating unions) or emulations of those (hierarchy of marker interfaces / abstract classes at the top 2 levels of the hierarchy)  .

# Checking for potential concurrency conflicts

We could do:

- Determine which events would be generated by the command (assuming it is valid) and optimistically save these "transient" events to the event store. It will raise a optimistic concurrency exception if the aggregates event stream is not at the expected version. Then we will get the events from the events stream since the expected version. Then we will do conflict resolution. In this case we can check each transient event in order against the events from the stream. Any event that does not conflict will be saved to the event store
