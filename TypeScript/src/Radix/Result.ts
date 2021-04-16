import { Addition } from "./Math/Pure/Algebra/Operations"
import { Monoid } from "./Math/Pure/Algebra/Structure"

export type Result<T, TError> = Ok<T> | Error<TError>

export type Ok<T> = { tag: "ok", value: T}
export type Error<TError> = { tag: "error", reason: TError }

export function ok<T>(t: T): Ok<T> { return { tag: "ok", value: t} }

export function error<T>(reason: T): Error<T> { return { tag: "error", reason: reason }}

export function bind<T, TError , U>(result: Result<T, TError>, f: (t: T) => Result<U, TError>): Result<U, TError> {
    switch (result.tag) {
        case "ok" : return f(result.value)
        case "error" : return result
    }
}

export function map<T, TError, U>(result: Result<T, TError>, f: (t: T) => U): Result<U, TError> {
    switch (result.tag) {
        case "ok" : return ok<U>(f(result.value))
        case "error" : return result
    }
}

export function mapError<T, TError, UError>(result: Result<T, TError>, f: (t: TError) => UError): Result<T, UError> {
    switch (result.tag) {
        case "ok" : return result
        case "error" : return error(f(result.reason))
    }
}

export function apply<T, TError extends Monoid<TError, Addition<TError>>, U>(fResult: Result<<T>(t: T) => U, TError>, xResult: Result<T, TError>): Result<U, TError> {

    switch ([fResult.tag, xResult.tag]) {
        case ["ok", "ok"] : return ok<U>((fResult as Ok<<T>(t: T) => U>).value((xResult as Ok<T>).value))
        case ["error", "error"] : return error<TError>((fResult as Error<TError>).reason.combine((xResult as Error<TError>).reason))
        default: return error<TError>((fResult as Error<TError>).reason)
    }
}

export default Result;
