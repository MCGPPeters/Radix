# Design Decisions

This document contains current ideas on the design directions. This can and will changed regularly. After they finalize they will be properly documented in the wiki.

## Aggregate migration

This can be done pretty easily. Since all events are stored centrally, the aggregate can be created on the other node and restore its state there. The binaries containing the logic for the context also contains the logic for the aggregates it contains.

## Task based user interfaces

Radix encourages a strict separation between command based user interface components, to let user execute tasks, and query based user interface components. These components can be combined in a composite user interface containing combinations of the two.

Query based components receive notifications of events from the event store and update their state in (near) real time. Speaking in terms of event sourcing, query based components are read models / projection based. These components set an initial state when first loading up. This initial state can come from any external source, for instance a backend read model. While these components are loaded, they will only receive updates from the event store that can be applied to the local state.

# Command and event hierarchies

Creating a hierarchy of events and commands helps determining the scope of those. At the top level of the command hierarchy is the overarching type of the bounded context. For instance within the bounded context of inventory management it could be InventoryCommand. The same goes for events : InventoryEvent. The second level of the hierarchy will be at the aggregate root level. Think of InventoryItemCommand and InventoryItemEvent. The easiest way of implementing these hierarchies is using sum types (discriminating unions) or emulations of those (hierarchy of marker interfaces / abstract classes at the top 2 levels of the hierarchy).

# Correlation and causation

When events are generated the id of the command (UUID) will be added as the causality id of the events generated as a consequence. Events have their own UUIDs. The correlation id of the command will propagate as the correlation id of the event.

# Communicating to the outside world

When an aggregate received a command, it might have to communicate to the outside world. Since construction of the aggregate is performed by the runtime (using the new() constraint), we cannot perform constructor injection. 

When a true concurrency error occurs (i.e. a command was issued and conflicts were found that could not be resolved according to domain specific conflict resolution logic) a way to handle the conflicts must be provided. Conflict resolution logic relieves us from the need of enforcing in order message delivery, which is impossible to solve technically. It will be done on a best effort basis.

# Versioning of events

The event store is responsible for assigning the version of an event when it is appended to the stream

# Versioning of event types

Versioning of event types is the responsibility of the user, however the library encourages / endorses to use a weak schema and use mapping and parsing in stead of deserializing. See Greg Young's https://leanpub.com/esversioning/read for details

# One event stream per aggregate

Each aggregate will have its own event stream. The id of the stream contains the full name of the dotnet type and the address of the aggregate (textually represented by a GUID)

# Traceability

An event will always contain a reference to the command that caused it to happen. Commands will all be logged, whether successful or not (status), including the reason of failure.

# Authentication / Authorization

Is not in scope for Radix... When the runtime received a command, it assumed the issuer of the command is authorized to do so. For auditing purposes it does however require information about the identity of the issuer of a command. The intention however is to runtime as secure as possible by default. This means before a command can be accepted by the aggregate actor, it needs to be an authorized command even if that means that anyone is authorized.

# Distributed architecture

A specific bounded context instance can be hosted on multiple nodes on the distributed runtime. A node can contain multiple bounded context instances. An aggregate can only exist on 1 (one) node at runtime, but can migrate to another node. A message for an aggregate can arrive at any node. When the aggregate is not found locally, the location will be looked up. If it can't be found in any node, the aggregate will be restored if it was ever created. Else an error occurs. A peer-to-peer architecture seems to be warranted.

The addresses of the aggregates can serve as the consistent hashes needed for finding the aggregates in the p2p overlay network.

The architecture is more grid oriented than cluster oriented since each different node has a different (set of) tasks, whereas a cluster acts as a single system performing the same task that is controlled and scheduled centrally.

# Open questions

- 



 

