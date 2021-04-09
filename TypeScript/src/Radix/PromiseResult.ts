import * as Result from "./Result";
import * as promise from "./Promise"

export type PromiseResult<T, TError> = Promise<Result.Result<T, TError>>

export async function bind<T, TResult, TError>(promiseResult: PromiseResult<T, TError>, f: (t: T) => PromiseResult<TResult, TError>): PromiseResult<TResult, TError>  {
    var result = await promiseResult;
    switch (result.tag) {
        case "ok" : return await f(result.value)
        case "error" : return result
    }
}

export async function map<T, TResult, TError>(promiseResult: PromiseResult<T, TError>, f: (t: T) => TResult): PromiseResult<TResult, TError>{

    var result = await promiseResult;
    switch (result.tag) {
        case "ok" : return Result.Ok<TResult>(f(result.value))
        case "error" : return result
    }
}

export function apply<T, TError extends { concat: <TError>(t: TError) => TError}, U>(fPromiseResult: PromiseResult<<T>(t: T) => U, TError>, xPromiseResult: PromiseResult<T, TError>): PromiseResult<U, TError> {

    return promise.bind(fPromiseResult, fResult =>
        promise.map(xPromiseResult, xResult =>
            Result.apply(fResult, xResult)))
}
