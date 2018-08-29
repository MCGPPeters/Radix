module internal AsyncPrimitives

open Radix
open Routing
open System.IO

type DeserializationError = DeserializationError of string

type Deserialize<'message> = Stream -> AsyncResult<'message, DeserializationError>

type SerializationError = SerializationError of string

type Serialize<'message> = 'message -> AsyncResult<Stream, SerializationError>

type Behavior<'state, 'message> = Deserialize<'message> -> 'state -> 'message -> Async<'state>

type InitialState<'state> = 'state

type Create<'state,'message> = Behavior<'state,'message> -> Actor

type Register = Registry -> Actor -> Registry

type New<'state,'message> = Register -> Create<'state,'message>

type Send<'message> = Serialize<'message> -> Route -> Address -> 'message -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

let register : Register = fun (Registry registry) actor ->
    Registry (registry.Add (actor.Address, actor.Agent))