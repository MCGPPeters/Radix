namespace Radix.Math.Applied.Learning.Neural

open System
open Radix.Math.Applied.Probability.Sampling
open MassTransit

type NodeId = NodeId of Randomized<Guid>

type InterNeuron = {
    Id: NodeId
}

// transmit signals from the central nervous system to the effector cells
// and are also called motor neurons.
type EfferentNeuron = {
    Id: NodeId
}

type MotorNeuron = EfferentNeuron

// convey information from tissues and organs into the central nervous system
// and are also called sensory neurons.
type AfferentNeuron = {
    Id : NodeId
}

type SensoryNeuron = AfferentNeuron

type Node =
| EfferentNeuron of EfferentNeuron
| InterNeuron of InterNeuron
| AfferentNeuron of AfferentNeuron
| Bias of float


type ConnectionId = ConnectionId of Randomized<NewId>

type Connection = {
    Id: ConnectionId
    Input: NodeId
    Output: NodeId
    Weight: float
}

module Connection =

    open Radix.Math.Applied.Probability

    let create id input output weightDistribution =
        match weightDistribution |> choose with
        | (Randomized (Some (Event weight))) ->
            Ok {
                Id = (ConnectionId id)
                Input = input
                Output = output
                Weight = weight
            }
        | Randomized None -> Error "The distribution of weights was empty"

type NetworkId = NetworkId of int

type Network = {
    Id: NetworkId
    Nodes : Node list
    Connections : Connection list
}
