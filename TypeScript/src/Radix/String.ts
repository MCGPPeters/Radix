import { Addition } from "./Math/Pure/Algebra/Operations"
import { Monoid } from "./Math/Pure/Algebra/Structure"

export type String = Monoid<string, Addition<string>>

export function create(s: string) : String {
    return {
        id: s,
        combine: (x: string) => s + x
    }
}

export function toString(s: String) {
    return s.id
}
