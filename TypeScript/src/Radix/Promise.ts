
import { Duration } from "@js-joda/core"

export async function bind<T, U>(option: Promise<T>, f: (t: T) => Promise<U>): Promise<U>  {
    return await f(await option)
}

export async function map<T, U>(promise: Promise<T>, f: (t: T) => U): Promise<U>{
    return f(await promise)
}

export function retry<T>(f: () => Promise<T>, ... intervals : Duration[]) : Promise<T> {
    return intervals.length === 0
        ? f()
        : f().catch(async () => {
            const duration = intervals.shift();
            if (typeof duration !== "undefined") {
                setTimeout(duration.toMillis);
                return retry(f, ...intervals);
            }
            else {
                return f();
            }
        })
}
