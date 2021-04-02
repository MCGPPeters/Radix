export default Result;

export type Result<T> = Ok<T> | Error

export type Ok<T> = { tag: "ok", value: T}
export type Error = { tag: "error", reason: string }

export function Ok<T>(t: T): Result<T> { return { tag: "ok", value: t} }

export function Error<T>(reason: string): Result<T> { return { tag: "error", reason: reason }}

export function bind<T, U>(result: Result<T>, f: (t: T) => Result<U>): Result<U> {
    switch (result.tag) {
        case "ok" : return f(result.value)
        case "error" : return result
    }
}

export function map<T, U>(result: Result<T>, f: (t: T) => U): Result<U>{
    switch (result.tag) {
        case "ok" : return Ok<U>(f(result.value))
        case "error" : return result
    }
}
