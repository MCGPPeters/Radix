export default Writer

export type Writer<A, W> = () => [value: A, Output: Array<W>]

export function unit<A, W>(a: A): Writer<A, W> {
    return () => [a, new Array<W>()]
}

export function bind<A, W, B>(writer: Writer<A, W>, f: (a: A) => Writer<B, W>): Writer<B, W>{
    return () => {
        const resultA = writer();
        const resultB = f(resultA[0])();

        return [resultB[0], resultA[1].concat(resultB[1])];
    }
}

export function map<A, W, B>(writer: Writer<A, W>, f: (a: A) => B): Writer<B, W>{
    return () => {
        const resultA = writer();
        const resultB = f(resultA[0]);

        return [resultB, resultA[1]];
    }
}
