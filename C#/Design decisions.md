# Aggregate migration

An instance of a bounded context can on multiple nodes simultaneously. An aggregate however can only live in 1 node at a time. It can however migrate to another node when certain criteria are met. This can be done pretty easily. Since all events are stored centrally, the aggregate can be created on the other node and restore its state there. The binaries containing the logic for the context also contains the logic for the aggregates it contains.

# Command and event hierarchies

Creating a hierarchy of events and commands helps determining the scope of those. At the top level of the command hierarchy is the overarching type of the bounded context. For instance withing the bounded context of inventory management it could be InventoryCommand. The same goes for events : InventoryEvent. The second level of the hierarchy will be at the aggregate root level. Think of InventoryItemCommand and InventoryItemEvent. The easiest way of implementing these hierarchies is using sum types (discriminating unions) or emulations of those (hierarchy of marker interfaces / abstract classes at the top 2 levels of the hierarchy)  .

# Checking for potential concurrency conflicts

A command is atomic. If there is a true concurrency conflict, no event that could be generated as a result of the command will be added to the stream.

The best approach for checking for and handling true concurrency conflicts (as opposed to technical optimistic concurrency exceptions) is to:

- Check if the expected version is still the current version of the event stream
  - If not, get all events since the expected version ( > the expected version) from the event stream. For each event check if there is a conflict according to business defined rules. 
    - If the are no conflicts, set the expected version to the latest version of the event stream and go to the next step. 
    - Otherwise there is a true concurrency conflict. Discard the command (and signal the issuer of the command => todo). END
- Evaluate the command and get the resulting transient events. Try to append these events to the event stream
  - If successful, we are END
  - If not, retry and go back to the first step

Radix assumes the event store properly supports optimistic consurrency

# Notifying issuers of command when concurrency errors occur

When a true concurrency error occurs (i.e. a command was issued and conflicts were found that could not be resolved according to domain specific conflict resolution logic) the issuers of the command is notified about the cause of the conflict. Conflict resolution logic relieves us from the need of enforcing in order message delivery, which is impossible to solve technically. It will be done on a best effort basis.

# Versioning of events

The event store is responsible for assigning the version of an event when it is appended to the stream

# One event stream per aggregate

Each aggregate will have its own event stream

# Traceability

An event will always contain a reference to the command that caused it to happen. Commands will all be logged, whether successful or not (status), including the reason of failure.

# Authentication / Authorization

Is not in scope for Radix... When the runtime received a command, it assumed the issuer of the command is authorized to do so. For auditing purposes it does however require information about the identity of the issuer of a command.

 

