namespace Radix

open System
open System.IO
open FSharp.Control.Reactive
open Radix

type Undefined = exn

type Address = Address of Guid

type LocalAddress = LocalAddress of Address

type RemoteAddress = RemoteAddress of Address

type UnknownAddress = UnknownAddress of Address

type ValidatedAddress = ValidatedAddress of Address

type UnvalidatedAddress = string

type UnvalidatedEnvelope = {
    UnvalidatedAddress: UnvalidatedAddress
    Message: Stream
}

type ValidatedEnvelope = {
    ValidatedAddress: ValidatedAddress
    Message: Stream
}

type InvalidEnvelopeError = InvalidEnvelopeError of string

type ExtractEnvelope = Stream -> Result<UnvalidatedEnvelope, InvalidEnvelopeError>

type InvalidAddressError = InvalidAddressError of string

type ValidateAddress = UnvalidatedAddress -> Result<ValidatedAddress, InvalidAddressError>

type ValidateEnvelope = ValidateAddress -> UnvalidatedEnvelope -> Result<ValidatedEnvelope, InvalidEnvelopeError>

type UnknownAddressError = UnknownAddressError of string

type RemoteRouteableEnvelope = {
    RemoteAddress: RemoteAddress
    Message: Stream
}

type LocallyRoutableEnvelope = {
    LocalAddress: LocalAddress
    Message: Stream
}

type RouteableEnvelope = 
    | LocallyRoutableEnvelope of LocallyRoutableEnvelope
    | RemoteRouteableEnvelope of RemoteRouteableEnvelope

type MailBox = Stream list

type MailboxRegistry = MailboxRegistry of Map<Address, MailBox>

type ResolveLocalAddress = ValidatedAddress -> Option<LocalAddress>

type ResolveRemoteAddress = ValidatedAddress -> Option<RemoteAddress>

type ResolveAddress = ResolveLocalAddress -> ResolveRemoteAddress -> ValidatedEnvelope -> Result<RouteableEnvelope, UnknownAddressError>



type UnableToForwardEnvelopeError = string

type UnableToPostEnvelopeError = string

type UnableToDeliverEnvelopeError = 
    | UnableToForwardEnvelopeError of UnableToForwardEnvelopeError
    | UnableToPostEnvelopeError of UnableToPostEnvelopeError



type EnvelopeDelivered = 
    | EnvelopePosted of LocallyRoutableEnvelope
    | EnvelopeForwarded of RemoteRouteableEnvelope

type PostEnvelope = MailboxRegistry -> LocallyRoutableEnvelope -> EnvelopeDelivered * MailboxRegistry

type ForwardEnvelope = RemoteRouteableEnvelope -> Result<EnvelopeDelivered, UnableToForwardEnvelopeError>
    
type DeliverEnvelope = ForwardEnvelope -> PostEnvelope -> MailboxRegistry -> RouteableEnvelope -> Result<EnvelopeDelivered * MailboxRegistry, UnableToDeliverEnvelopeError>

type Code = uint32

type NetworkProtocol =
| Ipv4
| Ipv6

type TransportProtocol =
| Udp
| Tcp

type ApplicationProtocol = 
| Http
| Https

type SessionProtocol = 
| Sockets

type DataLinkProtocol = 
| PPP

type DatalinkLayer = {
    Protocol: DataLinkProtocol
}

type NetworkLayer = {
    DatalinkLayer: DatalinkLayer option
    Protocol: NetworkProtocol
}

type TransportLayer = {
    NetworkLayer: NetworkLayer option
    Protocol : TransportProtocol
    Port: int16
}

type SessionLayer = {
    Protocol: SessionProtocol
}

type PresentationLayer = {
    SessionLayer: SessionLayer option
}

type ApplicationLayer = {
    PresentationLayer: PresentationLayer option
    Protocol: ApplicationProtocol
}

type Layer = 
| Application of ApplicationLayer
| Presentation of PresentationLayer
| Session of SessionLayer
| Transport of TransportLayer


type MultiAddress = Layer

type DeliverEnvelopeCommand = Command<UnvalidatedEnvelope>

type NodeCommands = 
| Deliver of DeliverEnvelopeCommand

type Listen = MultiAddress -> IObservable<DeliverEnvelopeCommand>

module Routing =

    let validateAddress : ValidateAddress = 
        fun unvalidatedAddress -> Ok (ValidatedAddress (Address(Guid.Parse(unvalidatedAddress))))

    let validateEnvelope : ValidateEnvelope = 
        fun validateAddress unvalidatedEnvelope ->
            match validateAddress unvalidatedEnvelope.UnvalidatedAddress with
                | Ok validatedAddress -> 
                    Ok {
                        ValidatedAddress = validatedAddress
                        Message = unvalidatedEnvelope.Message
                    }
                | _ -> Error (InvalidEnvelopeError "")

    let resolveAddress : ResolveAddress =
        fun resolveLocalAddress resolveRemoteAddress validatedEnvelope ->
            match resolveLocalAddress validatedEnvelope.ValidatedAddress with
            | Some localAddress -> Ok ( LocallyRoutableEnvelope {
                LocalAddress = localAddress
                Message = validatedEnvelope.Message
                })
            | None -> match resolveRemoteAddress validatedEnvelope.ValidatedAddress with
                        | Some remoteAddress -> Ok (RemoteRouteableEnvelope {
                            RemoteAddress = remoteAddress
                            Message= validatedEnvelope.Message
                        })
                        | None -> Error (UnknownAddressError "")

    let deliverEnvelope: DeliverEnvelope = 
        fun forwardEnvelope postEnvelope mailboxRegistry routeableEnvelope ->
            match routeableEnvelope with
            | LocallyRoutableEnvelope envelope -> 
                Ok (postEnvelope mailboxRegistry envelope)
            | RemoteRouteableEnvelope envelope ->
                match forwardEnvelope envelope with
                | Ok envelopDelivered -> Ok (envelopDelivered, mailboxRegistry)
                | Error error -> Error (UnableToForwardEnvelopeError error)

    let validateEnvelope' = validateAddress |> validateEnvelope

    let postEnvelope : PostEnvelope = 
        fun (MailboxRegistry mailboxRegistry) envelope -> 
            let (LocalAddress address) = envelope.LocalAddress
            let mailbox = mailboxRegistry.Item(address)
            (EnvelopePosted envelope, MailboxRegistry (mailboxRegistry.Add(address, envelope.Message :: mailbox)))

            
    //let forwardEnvelope : ForwardEnvelope = 
    //    fun envelope -> 
            

    let mailboxRegistry: MailboxRegistry = MailboxRegistry Map.empty<Address, MailBox>

    let deliverEnvelope' = deliverEnvelope forwardEnvelope postEnvelope mailboxRegistry

    open FSharp.Control.Reactive

    let unvalidateEnvelope = {
        UnvalidatedAddress = ""
        Message = Stream.Null
    }
    
    let listen : Listen = fun address -> Observable.toObservable [unvalidateEnvelope]

    let resolveLocalAddress (MailboxRegistry mailboxRegistry) : ResolveLocalAddress = 
        fun (ValidatedAddress address) ->
            match mailboxRegistry.TryFind(address) with
            | Some mailbox -> Some (LocalAddress address)
            | None -> None
    
    let resolveRemoteAddress : ResolveRemoteAddress = fun address -> No

    let resolveAdress' = resolveAddress resolveLocalAddress resolveRemoteAddress

    let route multiAddress = 
        multiAddress
        |> listen
        |> Observable.subscribe(fun unvalidatedEnvelope -> 
            unvalidatedEnvelope 
            |> validateEnvelope'
            |> resolveAddress'
            |> deliverEnvelope')
        |> ignore
        