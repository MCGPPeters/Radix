export type Binary<T> = (t: T) => T

export type Nullary<T> = () => T

export type Addition<T> = Binary<T>

export type Multiplication<T> = Binary<T>
