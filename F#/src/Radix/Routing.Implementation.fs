namespace Radix.Routing

module internal Implementation =

    open Radix
    open Radix.Routing.Types

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

    let route 
        resolveLocalAddress
        resolveRemoteAddress
        registry
        forward
        post
        : Route = fun envelope ->
           envelope
           |> resolve resolveLocalAddress resolveRemoteAddress
           |> AsyncResult.mapError (fun  (AddressNotFoundError error) -> UnableToDeliverEnvelopeError error)
           |> AsyncResult.bind (deliver registry forward post)

    let resolveLocalAddress (Registry registry) : ResolveLocalAddress = 
        fun envelope -> 
            match registry.TryFind envelope.Destination with
            | Some agent -> 
                Some {
                    Destination = envelope.Destination
                    Principal = envelope.Principal
                    Payload = envelope.Payload
                    Agent = agent
                }
            | _ -> None