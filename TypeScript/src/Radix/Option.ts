export type None = {
    tag: "none"
};

export type Some<T> = {
    tag: "some"
    value: T
}

export function Some<T>(t: T): Some<T> { return { tag: "some", value: t} }

export const None : None = {tag: "none"};

export type Option<T> = Some<T> | None

export function bind<T, U>(option: Option<T>, f: (t: T) => Option<U>): Option<U> {
    switch (option.tag) {
        case "some" : return f(option.value)
        case "none" : return None
    }
}

export function map<T, U>(option: Option<T>, f: (t: T) => U): Option<U>{
    switch (option.tag) {
        case "some" : return Some<U>(f(option.value))
        case "none" : return None
    }
}

export default Option
