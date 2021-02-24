
import { Duration } from "@js-joda/core"


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
