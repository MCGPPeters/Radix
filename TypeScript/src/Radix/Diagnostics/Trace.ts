import * as Result  from "../Result"
import * as PromiseResult  from "../PromiseResult"

export type Call<T, TError> = {
    name: string
    // the type of dependency, like HTTP of SQL
    type: string
    // detailed information on the call, such as the url or the SQL statement
    context: string
    // translate the
    getResultCode: (result: Result.Result<T, TError>) => string
}

export type Traceable<T, U, Error> = (t: T) => PromiseResult.PromiseResult<U, Error>

export type SendTrace<T, TError> = (result: Result.Result<T, TError>, call: Call<T, TError>, duration: number) => void

export const trace = <T, U, TError> (sendTrace: SendTrace<U, TError>) => (f: Traceable<T, U, TError>, call: Call<U, TError>): Traceable<T, U, TError> => {
    return async (t: T) => {
        const t1 = performance.now();
        const result = await f(t)
        const t2 = performance.now();
        sendTrace(result, call, t2 - t1);
        return result;
    }
}
