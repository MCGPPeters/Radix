namespace Radix.Routing

module Types =

    open System
    open Radix
    open System.IO
    open System.Security.Principal

    type Envelope<'message> = {
        Destination: Destination
        Principal: IPrincipal
        Payload: Payload<'message>
    }

    type AddressNotFoundError = AddressNotFoundError of string

    type ResolveLocalAddress = Address -> Option<LocalAddress>

    type ResolveRemoteAddress = Address -> AsyncResult<RemoteAddress, AddressNotFoundError>

    type Resolve = ResolveLocalAddress -> ResolveRemoteAddress -> Address -> AsyncResult<Destination, AddressNotFoundError>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    type EnvelopePosted<'message> = Event<'message>

    type EnvelopeForwarded<'message> = Event<'message>

    type EnvelopeDelivered<'message> = 
        | EnvelopePosted of EnvelopePosted<'message>
        | EnvelopeForwarded of EnvelopeForwarded<'message>

    type Deserialize<'message> = Stream -> 'message

    type Serialize<'message> = 'message -> Stream -> Stream

    type Post<'message> = Registry<'message> -> LocalAddress -> 'message -> EnvelopePosted<'message>

    type Forward<'message> = RemoteAddress -> 'message -> AsyncResult<EnvelopeForwarded<'message>, UnableToDeliverEnvelopeError>

    type Deliver<'message> =  Serialize<'message> -> Deserialize<'message> -> Forward<'message> -> Post<'message> -> RoutableEnvelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

    type Route<'message> = Envelope<'message> -> AsyncResult<EnvelopeDelivered<'message>, UnableToDeliverEnvelopeError>

            