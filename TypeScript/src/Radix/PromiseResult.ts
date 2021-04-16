import * as Result from "./Result";
import * as promise from "./Promise"
import { Monoid } from "./Math/Pure/Algebra/Structure";
import { Addition } from "./Math/Pure/Algebra/Operations";

export type PromiseResult<T, TError> = Promise<Result.Result<T, TError>>

export async function bind<T, TResult, TError>(promiseResult: PromiseResult<T, TError>, f: (t: T) => PromiseResult<TResult, TError>): PromiseResult<TResult, TError>  {
    var result = await promiseResult
    switch (result.tag) {
        case "ok" : return await f(result.value)
        case "error" : return result
    }
}

export async function map<T, TResult, TError>(promiseResult: PromiseResult<T, TError>, f: (t: T) => TResult): PromiseResult<TResult, TError> {

    var result = await promiseResult
    switch (result.tag) {
        case "ok" : return Result.ok<TResult>(f(result.value))
        case "error" : return result
    }
}

export function ok<T>(t:T): PromiseResult<T, any> {
    return Promise.resolve(Result.ok(t))
}

export function error<TError>(error:TError): PromiseResult<any, TError> {
    return Promise.resolve(Result.error(error))
}

export async function mapError<T, TError, UError>(promiseResult: PromiseResult<T, TError>, f: (t: TError) => UError): PromiseResult<T, UError> {
    var result = await promiseResult
    switch (result.tag) {
        case "ok" : return result
        case "error" : return error(f(result.reason))
    }
}

export function apply<T, TError extends Monoid<TError, Addition<TError>>, U>(fPromiseResult: PromiseResult<<T>(t: T) => U, TError>, xPromiseResult: PromiseResult<T, TError>): PromiseResult<U, TError> {

    return promise.bind(fPromiseResult, fResult =>
        promise.map(xPromiseResult, xResult =>
            Result.apply(fResult, xResult)))
}
