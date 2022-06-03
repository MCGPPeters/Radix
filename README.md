[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/MCGPPeters/Radix)
[![CI](https://github.com/MCGPPeters/Radix/actions/workflows/dotnet-core.yml/badge.svg?branch=main)](https://github.com/MCGPPeters/Radix/actions/workflows/dotnet-core.yml)
[![CodeQL](https://github.com/MCGPPeters/Radix/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/MCGPPeters/Radix/actions/workflows/codeql-analysis.yml)

# Radix

Radix is a set of libraries centered around building event sourced components and a runtime hosting those components. It treats event sourcing as a domain in itself. 

On top of that it provides libraries that make use of these components. A core example of this is a [library](https://github.com/MCGPPeters/Radix/wiki/Task-based-ASP-,NET-Components) that helps you build task oriented user interfaces based on ASP .NET Core (Blazor) components. It provides an alternate way of building ASP .NET Core (Blazor) components in functional style without using Razor components. It is heavily inspired by Elm and [Bolero](https://github.com/fsbolero/bolero) . It uses the 'model view update' pattern for building interactivity. It is heavily optimized to use the event sourcing library mentioned.

Al is build on top of a generic functional style core.

There is a C# version and a version in F#. Most effort at the time is in the C# version, though a goal is feature parity in C# and F#. For the F# equivalent of the Elm architecture I would recommend using [Bolero](https://github.com/fsbolero/bolero) .

Please consult the [wiki](https://github.com/MCGPPeters/Radix/wiki) for the docs, test and sample applications for guidance on how to use it.

# Motivation

The original end goal I had in mind is to build a library for building explainable AI systems. Using event sourcing concepts an 'audit trail' on how the system behaves and why should help analyze and understanding the behavior. Current idea is to mostly focus on reinforcement learning combined with neuro evolutional concepts, but the concept is not limited to only this application.

However, as mentioned in the intro, the generalized library for building event sourced components and task based user interfaces has most of the focus right now. 

# Status

A working of version of the event sourcing library is done.  Most focus is on building sample applications for testing realistic scenario's. Adding code generators for tedious work, like for value types and constrained value types with build in validation for preserving invariants.

