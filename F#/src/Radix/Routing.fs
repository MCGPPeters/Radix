module internal Routing

open System
open Radix
open System.IO
open System.Net
open System.Security.Principal
open Microsoft.FSharp.Control

type Envelope = {
        Destination: Address
        Principal: IPrincipal
        Payload: Stream
    }

type Listen = IPEndPoint -> IObservable<Envelope>

type RemoteRouteableEnvelope = {
    Destination: Address
    Principal: IPrincipal
    Payload: Stream
    Uri: Uri
}

type LocallyRoutableEnvelope = {
    Destination: Address
    Principal: IPrincipal
    Payload: Stream
    MailboxProcessor: MailboxProcessor<Stream>
}

type RouteableEnvelope = 
    | LocallyRoutableEnvelope of LocallyRoutableEnvelope
    | RemoteRouteableEnvelope of RemoteRouteableEnvelope

type AddressNotFoundError = AddressNotFoundError of string

type ResolveLocalAddress = Envelope -> Option<LocallyRoutableEnvelope>

type ResolveRemoteAddress = Envelope -> AsyncResult<RemoteRouteableEnvelope, AddressNotFoundError>

type Resolve = ResolveLocalAddress -> ResolveRemoteAddress -> Envelope -> AsyncResult<RouteableEnvelope, AddressNotFoundError>

type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

type EnvelopePosted = Event<LocallyRoutableEnvelope>

type EnvelopeForwarded = Event<RemoteRouteableEnvelope>

type EnvelopeDelivered = 
    | EnvelopePosted of EnvelopePosted
    | EnvelopeForwarded of EnvelopeForwarded

type Post = Registry -> LocallyRoutableEnvelope -> EnvelopePosted

type Forward = Uri -> RemoteRouteableEnvelope -> AsyncResult<EnvelopeForwarded, UnableToDeliverEnvelopeError>

type Deliver = Forward -> Post -> RouteableEnvelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

type Route = Envelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

        