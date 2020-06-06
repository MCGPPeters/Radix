# Radix

Radix is a set of libraries centered around building event sourced components and a runtime hosting those components. It treats event sourcing as a domain in itself. 

On top of that it provides libraries that make use of these components. An core example of this is a library that helps you build task oriented user interfaces based on Blazor components. It provides an alternate way of building Blazor components in functional style, heavily inspired by Elm. It uses the 'model view update' pattern for building interactivity. It is heavily optimized to use the event sourcing library mentioned.

All libraries are supported by general purpose components that encourage a more functional style of C# development.

# Motivation

The original end goal I had in mind is to build a library to build explainable AI systems. Using event sourcing concepts a 'audit trail' on how the system behaves and why should help analyze and understanding this behavior. Current idea is to mostly focus on reinforcement learning combined with neuro evolutional concepts, but the concept is not limited to only this application.

However, as mentioned in the intro, the generalized library for building event sourced components and task based user interfaces has most of the focus right now. 