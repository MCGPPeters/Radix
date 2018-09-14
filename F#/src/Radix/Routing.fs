namespace Radix.Routing

module Types =

    open System
    open Radix
    open System.IO
    open System.Security.Principal

    type Envelope<'message> = {
        Destination: Address
        Principal: IPrincipal
        Payload: Payload<'message>
    }

    type RemoteRoutableEnvelope<'message> = {
        Destination: Address
        Principal: IPrincipal
        Payload: Payload<'message>
        Uri: Uri
    }

    type LocallyRoutableEnvelope<'message> = {
        Destination: Address
        Principal: IPrincipal
        Payload: Payload<'message>
        Agent: Agent<'message>
    }

    type RoutableEnvelope<'message> = 
        | LocallyRoutableEnvelope of LocallyRoutableEnvelope<'message>
        | RemoteRouteableEnvelope of RemoteRoutableEnvelope<'message>

    type AddressNotFoundError = AddressNotFoundError of string

    type ResolveLocalAddress<'message> = Envelope<'message> -> Option<LocallyRoutableEnvelope<'message>>

    type ResolveRemoteAddress<'message> = Envelope<'message> -> AsyncResult<RemoteRoutableEnvelope<'message>, AddressNotFoundError>

    type Resolve<'message> = ResolveLocalAddress<'message> -> ResolveRemoteAddress<'message> -> Envelope<'message> -> AsyncResult<RoutableEnvelope<'message>, AddressNotFoundError>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type EnvelopePosted<'message> = Event<LocallyRoutableEnvelope<'message>>

    type EnvelopeForwarded<'message> = Event<RemoteRoutableEnvelope<'message>>

    type EnvelopeDelivered<'message> = 
        | EnvelopePosted of EnvelopePosted<'message>
        | EnvelopeForwarded of EnvelopeForwarded<'message>

    type Deserialize<'message> = Stream -> 'message

    type Serialize<'message> = 'message -> Stream -> Stream

    type Post<'message> = Registry<'message> -> 'message -> Address -> unit

    type Forward<'message> = Uri -> RemoteRoutableEnvelope<'message> -> AsyncResult<EnvelopeForwarded<'message>, UnableToDeliverEnvelopeError>

    type Deliver<'message> =  Serialize<'message> -> Deserialize<'message> -> Forward<'message> -> Post<'message> -> RoutableEnvelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

    type Route<'message> = Envelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

            