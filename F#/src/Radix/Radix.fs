namespace Radix

    type Undefined = exn

    type Ordering = LT | EQ | GT

    module Prelude =

        let const' x _ = x
