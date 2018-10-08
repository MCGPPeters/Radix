module Root

open System.IO

type Undefined = exn

type AsyncResult<'success, ' failure> = Async<Result<'success, 'failure>>

type Hash = Hash of byte[]

type Address = Address of Hash

module Address = 
    
    open System.Security.Cryptography
    open System

    let internal create (guid: Guid) = 
        let sha1 = SHA1.Create()
        Address (Hash (sha1.ComputeHash(guid.ToByteArray())))

type ValidationError = Undefined

type Validated<'t> = Validated of 't

type Validate<'t> = 't -> Result<Validated<'t>, ValidationError>

type Map<'a, 'b> = Validated<'a> -> 'b

type Envelope =
    abstract member Address: Address

type Envelope<'message> = {
    Address : Address
    Message : 'message
    }
    with interface Envelope

type DTO<'payload> = {
    Address : Address
    Payload : 'payload
    }

module TrustBoundary =

    type Deserialize<'payload> = Stream -> DTO<'payload>

    type Listen = unit -> Stream

    type Pack<'message> = Envelope<'message> -> Envelope

    module Pack = 

        let inline pack (envelope: Envelope<'message>) = envelope :> Envelope

    type Input<'payload, 'message> = Deserialize<'payload> -> Validate<'payload> -> Map<'payload, 'message> -> Pack<'message> -> Result<Envelope, ValidationError>

    type Serialize<'message> = Envelope<'message> -> MemoryStream -> Stream

    type Output<'message, 'payload> = Envelope<'message> -> DTO<'payload> -> Serialize<'message>

module Routing =

    open System

    type AddressNotFoundError = AddressNotFoundError of string

    type Resolve = Address -> AsyncResult<Uri, AddressNotFoundError>

    type EnvelopePosted = Event<Envelope>

    type EnvelopeForwarded = Event<Envelope>

    type Agent = Agent of MailboxProcessor<Envelope>

    type Registry = Registry of FSharp.Collections.Map<Address, Agent>

    type UnableToDeliverEnvelopeError = UnableToDeliverEnvelopeError of string

    open TrustBoundary

    type Forward = Uri -> Envelope -> AsyncResult<EnvelopeForwarded, UnableToDeliverEnvelopeError>
    

type BoundedContext = BoundedContext of MailboxProcessor<Envelope>

module BoundedContext =
 
    open Routing
    open System

    let create (resolve : Resolve) (forward: Forward) (Registry registry) =

        let context = BoundedContext (MailboxProcessor.Start(fun inbox -> 
                let rec messageLoop (Registry state) = async {
                    let! envelope = inbox.Receive()
                    match registry.TryFind envelope.Address with
                    | Some (Agent agent) -> 
                        agent.Post envelope
                    | _ -> 
                        let! resolveResult = resolve envelope.Address
                        match resolveResult with
                        | Ok uri-> 
                            let! forwardResult = forward uri envelope
                            forwardResult |> ignore // log
                        | Error error -> error |> ignore //log
                    return! messageLoop (Registry state)
                }
                    
                messageLoop (Registry registry)
                    
            )
        )
        
        context

module IO = 

    open TrustBoundary

    type PayloadAccepted<'payload> = Event<'payload>

    type PayloadDeclined<'payload> = Event<'payload>

    type Accept<'payload, 'message> = Input<'payload, 'message> -> BoundedContext -> Result<PayloadAccepted<'payload>, PayloadDeclined<'payload>>

    type Publish<'payload, 'message> = Output<'message, 'payload> -> Envelope<'message> -> unit


 

module Operators = 

    let (<--) = 