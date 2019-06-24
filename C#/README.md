# Radix

Radix is a set of components developed on top of a core runtime. It is a very opinionated runtime based principles from event sourcing, CQRS and the actor model.

The runtime is build around a few core concepts:

- Each aggregate is hosted in a statefull agent that has a unique address. That address is used 