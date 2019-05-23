# Aggregate migration

An instance of a bounded context can on multiple nodes simultaneously. An aggregate however can only live in 1 node at a time. It can however migrate to another node when certain criteria are met. This can be done pretty easily. Since all events are stored centrally, the aggregate can be created on the other node and restore its state there. The binaries containing the logic for the context also contains the logic for the aggregates it contains.

# Command and event hierarchies

Creating a hierarchy of events and commands helps determining the scope of those. At the top level of the command hierarchy is the overarching type of the bounded context. For instance withing the bounded context of inventory management it could be InventoryCommand. The same goes for events : InventoryEvent. The second level of the hierarchy will be at the aggregate root level. Think of InventoryItemCommand and InventoryItemEvent. The easiest way of implementing these hierarchies is using sum types (discriminating unions) or emulations of those (hierarchy of marker interfaces / abstract classes at the top 2 levels of the hierarchy)  .

# Checking for potential concurrency conflicts

A command is atomic. If there is a true concurrency conflict, no event that could be generated as a result of the command will be added to the stream.

The best approach for checking for true concurrency conflicts (as opposed to technical optimistic concurrency exceptions) is to:

- Check if the expected version is still the current version of the event stream
  - If not, get all events since the expected version ( > the expected version) from the event stream. For each event check if there is a conflict according to business defined rules. 
    - If the are no conflicts, go to the next step. 
    - Otherwise discard the command (and signal the issuer of the command => todo). END
- Evaluate the command and get the resulting transient events. Try to append these events to the event stream
  - If successful, we are END
  - If not, retry and go back to the first step

