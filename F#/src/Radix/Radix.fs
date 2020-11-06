namespace Radix

    type Undefined = exn

    type Ordering = LT | EQ | GT

    module Prelude =

        let inline const' k = fun _ -> k

        let memoize fn =
          let cache = new System.Collections.Generic.Dictionary<_,_>()
          (fun x ->
            match cache.TryGetValue x with
            | true, v -> v
            | false, _ -> let v = fn (x)
                          cache.Add(x,v)
                          v)
