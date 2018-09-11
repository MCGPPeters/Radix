namespace Radix

open System.IO
open Radix.Routing
open Radix.Routing.Implementation
open System

module Primitives =

    type DeserializationError = DeserializationError of string

    type Deserialize<'message> = Stream -> AsyncResult<'message, DeserializationError>

    type SerializationError = SerializationError of string

    type Serialize<'message> = 'message -> AsyncResult<Stream, SerializationError>

    type Behavior<'state, 'message> = Deserialize<'message> -> 'state -> 'message -> Async<'state>

    type ActorCreated = {
        Actor: Actor
    }

    type ActorCreatedEvent = Event<ActorCreated>

    type ActorEvent = 
    | ActorCreated of ActorCreatedEvent

    type NodeMessage =
    | ActorEvent of ActorEvent
    | Envelope of Envelope

    type Node = private Node of MailboxProcessor<NodeMessage>

    let post: Post =
                fun (Registry registry') envelope -> 
                    let entry = registry'.Item envelope.Destination
                    entry.Post envelope.Payload
                    {
                        Aggregate = envelope.Destination
                        Payload = envelope
                        Timestamp = DateTime.Now
                    } 

    module Node = 
        let create (Registry registry) (forward: Forward) =
            

            MailboxProcessor.Start(fun inbox ->
                let rec messageLoop state = async {
                    let! message = inbox.Receive()

                    match message with
                    | ActorEvent actorEvent ->
                        match actorEvent with
                        | ActorCreated actorCreated ->
                             return! messageLoop (registry.Add (actorCreated.Actor.Address, actorCreated.Actor.Agent))
                    | Envelope envelope ->
                        let agent = deliver registry envelope                          
                }

                messageLoop registry
            )

    

    type InitialState<'state> = 'state

    type Create<'state,'message> = Behavior<'state,'message> -> ActorCreated

    type Register = Registry -> Actor -> Registry

    type Send<'message> = Serialize<'message> -> Route -> Address -> 'message -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

    let register : Register = fun (Registry registry) actor ->
        Registry (registry.Add (actor.Address, actor.Agent))