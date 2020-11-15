[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/MCGPPeters/Radix)
![.NET Core](https://github.com/MCGPPeters/Radix/workflows/.NET%20Core/badge.svg)
![CodeQL](https://github.com/MCGPPeters/Radix/workflows/CodeQL/badge.svg)

# Radix

Radix is a set of libraries centered around building event sourced components and a runtime hosting those components. It treats event sourcing as a domain in itself. 

On top of that it provides libraries that make use of these components. A core example of this is a [library](https://github.com/MCGPPeters/Radix/wiki/Task-based-ASP-,NET-Components) that helps you build task oriented user interfaces based on ASP .NET Core (Blazor) components. It provides an alternate way of building ASP .NET Core (Blazor) components in functional style without using Razor components. It is heavily inspired by Elm. It uses the 'model view update' pattern for building interactivity. It is heavily optimized to use the event sourcing library mentioned.

Al is build on top of a generic functional style core.

There is a C# version and a version in F#. Most effort at the time is in the C# version, though a goal is feature parity in C# and F#. For the F# equivalent of the Elm architecture I would recommend using [Bolero](https://github.com/fsbolero/bolero) .

Please consult the [wiki](https://github.com/MCGPPeters/Radix/wiki) for the docs, test and sample applications for guidance on how to use it.

# Motivation

The original end goal I had in mind is to build a library for building explainable AI systems. Using event sourcing concepts a 'audit trail' on how the system behaves and why should help analyze and understanding this behavior. Current idea is to mostly focus on reinforcement learning combined with neuro evolutional concepts, but the concept is not limited to only this application.

However, as mentioned in the intro, the generalized library for building event sourced components and task based user interfaces has most of the focus right now. 

# Status

A working of version of the event sourcing library is done. Most focus in on trying to build more tests, sample applications, docs and produce alpha NuGet package versions of the C# libraries.

# JetBrains support

I would like to thank the people at JetBrains for supporting this project with open source licenses of all their products.

[![Foo](https://raw.githubusercontent.com/MCGPPeters/Radix/develop/jetbrains.svg)](https://www.jetbrains.com/?from=Radix)
