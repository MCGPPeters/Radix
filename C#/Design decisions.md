# Aggregate migration

An instance of a bounded context can on multiple nodes simultaneously. An aggregate however can only live in 1 node at a time. It can however migrate to another node when certain criteria are met. This can be done pretty easily. Since all events are stored centrally, the aggregate can be created on the other node and restore its state there. The binaries containing the logic for the context also contains the logic for the aggregates it contains.

# Command and event hierarchies

Creating a hierarchy of events and commands helps determining the scope of those. At the top level of the command hierarchy is the overarching type of the bounded context. For instance withing the bounded context of inventory management it could be InventoryCommand. The same goes for events : InventoryEvent. The second level of the hierarchy will be at the aggregate root level. Think of InventoryItemCommand and InventoryItemEvent. The easiest way of implementing these hierarchies is using sum types (discriminating unions) or emulations of those (hierarchy of marker interfaces / abstract classes at the top 2 levels of the hierarchy).

# Correlation and causation

When events are generated the id of the command (UUID) will be added as the causality id of the events generated as a consequence. Events have their own UUIDs. The correlation id of the command will propagate as the correlation id of the event.

# Checking for potential concurrency conflicts

A command is atomic. If there is a true concurrency conflict, no event that could be generated as a result of the command will be added to the stream.

The best approach for checking for and handling true concurrency conflicts (as opposed to technical optimistic concurrency exceptions) is to:

- Check if the expected version is still the current version of the event stream
  - If not, get all events since the expected version ( > the expected version) from the event stream. For each event check if there is a conflict according to business defined rules (conflict resolution). 
    - If the are no conflicts, set the expected version to the latest version of the event stream and go to the next step. 
    - Otherwise there is a true concurrency conflict. Discard the command (and signal the issuer of the command => todo). END
- Evaluate the command and get the resulting transient events. Try to append these events to the event stream
  - If successful, we are END
  - If not, retry and go back to the first step

Radix assumes the event store properly supports optimistic concurrency

# Communicating to the outside world

When an aggregate received a command, it might have to communicate to the outside world. Since construction of the aggregate is performed by the runtime (using the new() constraint), we cannot perform constructor injection. 

When a true concurrency error occurs (i.e. a command was issued and conflicts were found that could not be resolved according to domain specific conflict resolution logic) a way to handle the conflicts must be provided. Conflict resolution logic relieves us from the need of enforcing in order message delivery, which is impossible to solve technically. It will be done on a best effort basis.

When creating an aggregate, a record with settings can be provided. This will contain a method for handling concurrency conflicts as a minimum. When handling the command, the settings will be sent along with the command

# Versioning of events

The event store is responsible for assigning the version of an event when it is appended to the stream

# One event stream per aggregate

Each aggregate will have its own event stream

# Traceability

An event will always contain a reference to the command that caused it to happen. Commands will all be logged, whether successful or not (status), including the reason of failure.

# Authentication / Authorization

Is not in scope for Radix... When the runtime received a command, it assumed the issuer of the command is authorized to do so. For auditing purposes it does however require information about the identity of the issuer of a command.

# Distributed architecture

A specific bounded context instance can be hosted on multiple nodes on the distributed runtime. A node can contain multiple bounded context instances. An aggregate can only exist on 1 (one) node at runtime, but can migrate to another node. A message for an aggregate can arrive at any node. When the aggregate is not found locally, the location will be looked up. If it can't be found in any node, the aggregate will be restored if it was ever created. Else an error occurs. A peer-to-peer architecture seems to be warranted.

The addresses of the aggregates can serve as the consistent hashes needed for finding the aggregates in the p2p overlay network.

The architecture is more grid oriented than cluster oriented since each different node has a different (set of) tasks, whereas a cluster acts as a single system performing the same task that is controlled and scheduled centrally.

# Open questions

- 



 

