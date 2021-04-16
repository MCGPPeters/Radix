import { Addition, Binary, Multiplication } from "./Operations";

export type Semigroup<A, B extends Binary<A>> = {
    combine: B
}

export type Monoid<A, B extends Binary<A>> = Semigroup<A, B> & {
    id: A
}

export type Group< A, B extends Binary<A>> = Monoid<A, B> & {
    invert: B
}

export type Ring<A> = Monoid<A, Multiplication<A>> & Group<A, Addition<A>>

export type Field<A> = Ring<A>
