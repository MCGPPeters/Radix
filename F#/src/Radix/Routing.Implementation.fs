namespace Radix.Routing

module internal Implementation =

    open Radix
    open Radix.Routing.Types
    open System
    open System.IO

    

    let post: Post<'message> =
            fun (Registry registry') message (destination: Address) -> 
                let (Agent entry) = registry'.Item destination
                entry.Post message

    let deliver mailboxes : Deliver<'message> =
        fun serialize deserialize forward post routeableEnvelope ->
            match routeableEnvelope with
                | LocallyRoutableEnvelope envelope -> 
                    let message = match envelope.Payload with
                                  | Stream stream -> deserialize stream
                                  | Message message' -> message'
                    post mailboxes message envelope.Destination
                    {
                        Timestamp = DateTimeOffset.Now
                        Payload = message
                    } 
                    |> EnvelopePosted 
                    |> AsyncResult.retn
                | RemoteRouteableEnvelope envelope ->
                    let stream = new MemoryStream() 
                    let envelope' = match envelope.Payload with
                                    | Stream _ -> envelope
                                    | Message message' -> 
                                        let s = serialize message' stream
                                        {envelope with Payload = (Radix.Stream s)}
                    
                    forward envelope.Uri envelope'
                        |> AsyncResult.map EnvelopeForwarded

    let resolveLocalAddress (Registry registry) : ResolveLocalAddress<'message> = 
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

    let resolve : Resolve<'message> = 
        fun resolveLocalAddress resolveRemoteAddress envelope ->
            match resolveLocalAddress envelope with
                | Some locallyRoutableEnvelope -> AsyncResult.retn (LocallyRoutableEnvelope locallyRoutableEnvelope)
                | None -> 
                    resolveRemoteAddress envelope 
                    |> AsyncResult.map RemoteRouteableEnvelope

    let route 
        resolveRemoteAddress
        registry
        forward
        serialize
        deserialize
        : Route<'message> = fun envelope ->
           envelope
           |> resolve (resolveLocalAddress registry) resolveRemoteAddress
           |> AsyncResult.mapError (fun  (AddressNotFoundError error) -> UnableToDeliverEnvelopeError error)
           |> AsyncResult.bind (deliver registry forward serialize deserialize post)

    