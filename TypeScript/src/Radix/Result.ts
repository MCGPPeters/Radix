export type Result<T, TError> = Ok<T> | Error<TError>

export type Ok<T> = { tag: "ok", value: T}
export type Error<TError> = { tag: "error", reason: TError }

export function Ok<T>(t: T): Ok<T> { return { tag: "ok", value: t} }

export function Error<T extends { concat: <T>(t: T) => T}>(reason: T): Error<T> { return { tag: "error", reason: reason }}

export function bind<T, TError , U>(result: Result<T, TError>, f: (t: T) => Result<U, TError>): Result<U, TError> {
    switch (result.tag) {
        case "ok" : return f(result.value)
        case "error" : return result
    }
}

export function map<T, TError, U>(result: Result<T, TError>, f: (t: T) => U): Result<U, TError> {
    switch (result.tag) {
        case "ok" : return Ok<U>(f(result.value))
        case "error" : return result
    }
}

export function apply<T, TError extends { concat: <TError>(t: TError) => TError}, U>(fResult: Result<<T>(t: T) => U, TError>, xResult: Result<T, TError>): Result<U, TError> {

    switch ([fResult.tag, xResult.tag]) {
        case ["ok", "ok"] : return Ok<U>((fResult as Ok<<T>(t: T) => U>).value((xResult as Ok<T>).value))
        case ["error", "error"] : return Error<TError>((fResult as Error<TError>).reason.concat((xResult as Error<TError>).reason))
        default: return Error<TError>((fResult as Error<TError>).reason)
    }
}

export default Result;
