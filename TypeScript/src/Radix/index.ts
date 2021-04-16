export * as Writer from "./Writer";
export * as Result from "./Result";
export * as PromiseResult from "./PromiseResult";
export * as Option from "./Option";

export const memoize = <T, U>(f: (t: T) => U) => {
    const cache: Map<string, U> = new Map<string, U>();

    return (t: T) => {
        const args = JSON.stringify(t);
        const result = cache.get(args);
        switch(typeof result){
            case "undefined": {
                const y = f(t);
                cache.set(args, y);
                return y;
            }
            default:  return result
        }
    }
}
