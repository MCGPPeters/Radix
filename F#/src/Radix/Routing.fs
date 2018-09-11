namespace Radix

module internal Routing =

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

    type RemoteRoutableEnvelope = {
        Destination: Address
        Principal: IPrincipal
        Payload: Stream
        Uri: Uri
    }

    type LocallyRoutableEnvelope = {
        Destination: Address
        Principal: IPrincipal
        Payload: Stream
        Agent: Agent
    }

    type RoutableEnvelope = 
        | LocallyRoutableEnvelope of LocallyRoutableEnvelope
        | RemoteRouteableEnvelope of RemoteRoutableEnvelope

    type AddressNotFoundError = AddressNotFoundError of string

    type ResolveLocalAddress = Envelope -> Option<LocallyRoutableEnvelope>

    type ResolveRemoteAddress = Envelope -> AsyncResult<RemoteRoutableEnvelope, AddressNotFoundError>

    type Resolve = ResolveLocalAddress -> ResolveRemoteAddress -> Envelope -> AsyncResult<RoutableEnvelope, AddressNotFoundError>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type EnvelopePosted = Event<LocallyRoutableEnvelope>

    type EnvelopeForwarded = Event<RemoteRoutableEnvelope>

    type EnvelopeDelivered = 
        | EnvelopePosted of EnvelopePosted
        | EnvelopeForwarded of EnvelopeForwarded

    type Post = Registry -> LocallyRoutableEnvelope -> EnvelopePosted

    type Forward = Uri -> RemoteRoutableEnvelope -> AsyncResult<EnvelopeForwarded, UnableToDeliverEnvelopeError>

    type Deliver = Forward -> Post -> RoutableEnvelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

    type Route = Envelope -> AsyncResult<EnvelopeDelivered, UnableToDeliverEnvelopeError>

            