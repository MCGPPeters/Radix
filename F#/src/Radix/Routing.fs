module Routing

open System
open Radix
open System.IO
open System.Net
open System.Security.Principal
open FSharp.Control.Reactive
open Microsoft.FSharp.Control

type Envelope = {
        Origin: Address
        Destination: Address
        Principal: IPrincipal
        Payload: Stream
    }

type Listen = IPEndPoint -> IObservable<Envelope>

type RemoteRouteableEnvelope = {
    Origin: Address
    Destination: Address
    Principal: IPrincipal
    Payload: Stream
    Uri: Uri
}

type LocallyRoutableEnvelope = {
    Origin: Address
    Destination: Address
    Principal: IPrincipal
    Payload: Stream
}

type RouteableEnvelope = 
    | LocallyRoutableEnvelope of LocallyRoutableEnvelope
    | RemoteRouteableEnvelope of RemoteRouteableEnvelope

type AddressNotFoundError = AddressNotFoundError of string

type ResolveLocalAddress = Envelope -> Option<LocallyRoutableEnvelope>

type ResolveRemoteAddress = Envelope -> AsyncResult<RemoteRouteableEnvelope, AddressNotFoundError>

type Resolve = ResolveLocalAddress -> ResolveRemoteAddress -> Envelope -> AsyncResult<RouteableEnvelope, AddressNotFoundError>

type UnableToForwardEnvelopeError = UnableToForwardEnvelopeError of string

type UnableToPostEnvelopeError = UnableToPostEnvelopeError of string

type UnableToDeliverEnvelopeError = 
    | UnableToForwardEnvelopeError of string
    | UnableToPostEnvelopeError of string

type EnvelopePosted = Event<LocallyRoutableEnvelope>

type EnvelopeForwarded = Event<RemoteRouteableEnvelope>

type EnvelopeDelivered = 
    | EnvelopePosted of EnvelopePosted
    | EnvelopeForwarded of EnvelopeForwarded

type MailBox = {
    Address: Address
    MailboxProcessor: MailboxProcessor<Stream>
}

type Post = MailBox list -> LocallyRoutableEnvelope -> EnvelopePosted

type Forward = Uri -> RemoteRouteableEnvelope -> AsyncResult<EnvelopeForwarded, UnableToForwardEnvelopeError>

type Deliver = Forward -> Post -> RouteableEnvelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

type Route = Envelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

let resolve : Resolve = 
    fun resolveLocalAddress resolveRemoteAddress envelope ->
        match resolveLocalAddress envelope with
            | Some locallyRoutableEnvelope -> AsyncResult.retn (LocallyRoutableEnvelope locallyRoutableEnvelope)
            | None -> 
                resolveRemoteAddress envelope 
                |> AsyncResult.map RemoteRouteableEnvelope

let deliver mailboxes : Deliver =
    fun forward post routeableEnvelope ->
        match routeableEnvelope with
            | LocallyRoutableEnvelope envelope -> 
                post mailboxes envelope
                |> EnvelopePosted
                |> AsyncResult.retn
            | RemoteRouteableEnvelope envelope ->
                forward envelope.Uri envelope
                    |> AsyncResult.map EnvelopeForwarded
                    |> AsyncResult.mapError (fun error -> UnableToForwardEnvelopeError "")

let route 
    resolveLocalAddress
    resolveRemoteAddress
    mailboxes
    forward
    post
    : Route = fun envelope ->
       envelope
       |> resolve resolveLocalAddress resolveRemoteAddress
       |> AsyncResult.mapError (fun  (AddressNotFoundError error) -> UnableToForwardEnvelopeError error)
       |> AsyncResult.bind (deliver mailboxes forward post)