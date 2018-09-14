namespace Radix.Routing

module Types =

    open System
    open Radix
    open System.IO
    open System.Security.Principal


    type Envelope<'message> = {
        Destination: Address
        Principal: IPrincipal
        Payload: 'message
    }

    type RemoteRoutableEnvelope = {
        Destination: Address
        Principal: IPrincipal
        Payload: Stream
        Uri: Uri
    }

    type LocallyRoutableEnvelope<'message> = {
        Destination: Address
        Principal: IPrincipal
        Payload: 'message
        Agent: Agent<'message>
    }

    type RoutableEnvelope<'message> = 
        | LocallyRoutableEnvelope of LocallyRoutableEnvelope<'message>
        | RemoteRouteableEnvelope of RemoteRoutableEnvelope

    type AddressNotFoundError = AddressNotFoundError of string

    type ResolveLocalAddress<'message> = Envelope<'message> -> Option<LocallyRoutableEnvelope<'message>>

    type ResolveRemoteAddress<'message> = Envelope<'message> -> AsyncResult<RemoteRoutableEnvelope, AddressNotFoundError>

    type Resolve<'message> = ResolveLocalAddress<'message> -> ResolveRemoteAddress<'message> -> Envelope<'message> -> AsyncResult<RoutableEnvelope<'message>, AddressNotFoundError>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type EnvelopePosted<'message> = Event<LocallyRoutableEnvelope<'message>>

    type EnvelopeForwarded = Event<RemoteRoutableEnvelope>

    type EnvelopeDelivered<'message> = 
        | EnvelopePosted of EnvelopePosted<'message>
        | EnvelopeForwarded of EnvelopeForwarded

    type Deserialize<'message> = Stream -> 'message

    type Post<'message> = Registry<'message> -> Deserialize<'message> -> LocallyRoutableEnvelope<'message> -> EnvelopePosted<'message>

    type Forward = Uri -> RemoteRoutableEnvelope -> AsyncResult<EnvelopeForwarded, UnableToDeliverEnvelopeError>

    type Deliver<'message> = Forward -> Post<'message> -> RoutableEnvelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

    type Route<'message> = Envelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

            