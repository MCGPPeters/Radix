namespace Radix

open System.IO
open Radix.Routing
open Radix.Routing.Implementation
open System

module Primitives =

    type DeserializationError = DeserializationError of string

    type Deserialize<'message> = Stream -> AsyncResult<'message, DeserializationError>

    type SerializationError = SerializationError of string

    type Serialize<'message> = 'message -> AsyncResult<Stream, SerializationError>

    type Behavior<'state, 'message> = 'state -> 'message -> Async<'state>

    let post: Post =
                fun (Registry registry') envelope -> 
                    let (Agent entry) = registry'.Item envelope.Destination
                    entry.Post envelope.Payload
                    {
                        Aggregate = envelope.Destination
                        Timestamp = DateTimeOffset.Now
                        Payload = envelope
                    } 

    type NodeMessage =
    | Envelope of Envelope

    type Node = Node of MailboxProcessor<NodeMessage>

    type Send<'message> = 'message -> Async<unit>

    type SendMessage<'message> = Node -> Serialize<'message> -> Address -> 'message -> Async<unit>
    
    type New<'state, 'message> = Behavior<'state, 'message> -> Send<'message> 

    type InitialState<'s> = InitialState of 's

    

    type Create<'state, 'message> = Deserialize<'message> -> Serialize<'message> -> Behavior<'state, 'message> -> InitialState<'state> -> Agent

    let create : Create<'state, 'message> = fun deserialize serialize behavior (InitialState initialState) ->
         Agent (MailboxProcessor.Start(fun inbox ->
                let rec messageLoop state = async {
                    let! stream = inbox.Receive()
                    let! deserializationResult = deserialize stream
                    match deserializationResult with
                    | Ok message -> 
                        let! newState = behavior state message
                        return! messageLoop newState
                    | Error error -> //log
                        return! messageLoop state                                 
                }

                messageLoop initialState
            ))

    module Node = 

        let create registry (resolveRemoteAddress: ResolveRemoteAddress) (forward: Forward) =
            
            Node (MailboxProcessor.Start(fun inbox ->
                let rec messageLoop (Registry state) = async {
                    let! message = inbox.Receive()

                    match message with
                    | Envelope envelope ->
                        asyncResult {
                            let! deliveryResult = route (resolveLocalAddress registry) resolveRemoteAddress registry forward post envelope
                            match deliveryResult with
                            | EnvelopePosted posted ->  posted |> ignore //logging
                            | EnvelopeForwarded forwarded ->  forwarded |> ignore //logging
                        } |> ignore
                        return! messageLoop (Registry state)
                }

                messageLoop registry
            ))     

    let send : SendMessage<'message> = fun (Node node) serialize address message ->
            asyncResult {
                let! payload = serialize message
                let envelope = Envelope {
                    Destination =  address
                    Principal = Threading.Thread.CurrentPrincipal
                    Payload = payload
                }
                node.Post envelope
             }
             |> Async.map (fun x-> 
                match x with
                | Ok ok -> ok |> ignore
                | Error error -> error |> ignore) //logging