namespace Radix

open System.Collections.Concurrent
open System.IO
open System

type undefined = exn

type MultiAddress = undefined

type Address = undefined

type MailBox = {
    Messages: Stream list
}

type Envelope = {
    Destination: Address
    Message: Stream
}

type SerializationError = SerializationError of undefined

type Deserialize<'m> = Stream -> Result<'m, SerializationError>

type Serialize<'m> = 'm -> Stream

type Actor = {
    Address: Address
    MailBox: MailBox
}

type Node = {
    Address: MultiAddress
    Actors: Actor list
}



module MailBox =

    



module Node = 

    type Resolve = Address -> Node

    type TransportError = TransportError of undefined

    type Listen = Stream -> MultiAddress -> IObservable<Envelope>

    type Route = IObservable<Envelope> -> MailBox
    
    type Origin = Origin of Node
    
    type Send = Origin -> Envelope -> Async<Result<Unit, TransportError>>


module Actor =

    type Send<'m> = 'm -> Address -> unit

    type Behavior<'s, 'm> = 's -> 'm -> Async<'s>

    type Create = 





type Create<'s, 'm> = Deserialize<'m> -> MailboxProcessor<'m> -> Behavior<'s, 'm> -> Address




type Resolve = Address -> MultiAddress

type SendError = SendError of undefined

type Send<'m> = Resolve -> Serialize<'m> -> Address -> 'm -> Async<Result<Unit, SendError>>

type Deserialize<'m> -> 
