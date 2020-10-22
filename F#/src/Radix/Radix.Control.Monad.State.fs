namespace Radix.Control.Monad.State

type Statefull<'State,'Value> = Statefull of ('State -> 'Value * 'State)

module Statefull =

    let run (Statefull f) state = f state

    let return' x =
        let run state =
            x, state
        Statefull run

    let map f statefull =
        let run state =
            let x, newState = run statefull state
            f x, newState
        Statefull run

    let bind f statefull =
        let run state =
            let x, newState = run statefull state
            run (f x) newState
        Statefull run

    let getState =
        let run state =
            state, state
        Statefull run

    let putState newState =
        let run _ =
            (), newState
        Statefull run

    type StateBuilder()=
        member inline __.Zero() = return' ()
        member inline __.Return(x) = return' x
        member inline __.ReturnFrom(statefull) = statefull
        member inline __.Bind(statefull,f) = bind f statefull
        member inline __.Map(statefull, f) = map f statefull

    let state = new StateBuilder()

module Stack =

    open Statefull

    type Stack<'a> = Stack of 'a list

    // define pop outside of state expressions
    let popStack (Stack contents) =
        match contents with
        | [] -> failwith "Stack underflow"
        | head::tail ->
            head, (Stack tail)

    // define push outside of state expressions
    let pushStack newTop (Stack contents) =
        Stack (newTop::contents)

    // define an empty stack
    let emptyStack = Stack []

    // get the value of the stack when run
    // starting with the empty stack
    let getValue stackM =
        run stackM emptyStack |> fst

    let pop() = state {
        let! stack = getState
        let top, remainingStack = popStack stack
        do! putState remainingStack
        return top
    }

    let push newTop = state {
        let! stack = getState
        let newStack = pushStack newTop stack
        do! putState newStack
        return ()
    }
