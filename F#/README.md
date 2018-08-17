# Radix

context: Communication Reception

// ----------------------
// Simple types
// ----------------------

data Address = GloballyUniqueIdentifier

// ----- unvalidated state -----

data UnvalidatedCommunication =
    Stream

data Address = 
    UnvalidatedAddress
    OR ValidatedAddress

data UnvalidateAddress = ...

data ValidatedAddress = 
    KnownAddress
    OR UnknownAddress

data KnownAddress = 
    LocalAddress
    OR RemoteAddress

// ----- validated state -----
data Envelope =
    ValidatedAddress
    AND Message

// ----- output events -----
data EnvelopeDelivered =
    Envelope

data EnvelopeForwarded =
    Envelope

// ----------------------
// Processes
// ----------------------

process "Route Communication" =
    input: UnvalidatedCommunication
    output (on success):
        data EnvelopeDelivered (address is local)
        OR EnvelopeForwarded (address is not found locally but does exist somewhere else)
    output (on error):
        InvalidEnvelopeError
        InvalidAddressError
        UnknownAddressError
        UnableToForwardEnvelopeError

    do ExtractEnvelope
        input: Stream
        output: Envelope OR InvalidEnvelopeError
        if stream does not contain a valid envelope
            return with InvalidEnvelopeError

    do ValidateAddress
        input: Envelope
        output: ValidatedAddress 
                OR InvalidAddressError
        if address is not valid
            return with InvalidAddressError

    do ResolveAddress
        input: ValidatedAddress
        output: KnownAddress 
                OR UnknownAddressError
        if ValidatedAddress is not found
            return with UnknowAddressError

    do RouteEnvelope
        if KnownAddress is LocalAddress
            do PostEnvelope
                input: LocalAddress
                output: EnvelopeDelivered OR (Error?)
        if KnownAddress is RemoteAddress
            do ForwardEnvelope
                input: RemoteAddress
                output: EnvelopeForwarded 
                        OR UnableToForwardEnvelopeError
                if Envelope can not be forwarded
                    return with UnableToForwardEnvelopeError

    create and return events
