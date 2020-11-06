namespace Radix.Collections

    module List =

        let rec iterate f value = seq { yield value; yield! iterate f (f value) }

        let rec findFirst predicate list =
            match list with
            | [] -> []
            | x::_ when predicate x -> [x]
            | _::xs -> findFirst predicate xs

        let rec update original replacement list =
            match list with
            | [] -> []
            | x::xs when x = original -> replacement :: xs
            | x::xs -> x :: update original replacement xs

        let rec allign xs ys compareBy =
            let xs = xs |> List.sortBy compareBy
            let ys = ys |> List.sortBy compareBy
            match (xs, ys) with
            | [], ys -> List.map (fun z -> (None, Some z)) ys
            | xs, [] -> List.map (fun z -> (None, Some z)) xs
            | x :: xs, y :: ys ->
                match (x, y) with
                | (xl, yl) when compareBy xl = compareBy yl -> (Some x, Some y) :: allign xs ys compareBy
                | (xl, yl) when compareBy xl < compareBy yl -> (Some x, None) :: allign xs (y::ys) compareBy
                | (xl, yl) when compareBy xl > compareBy yl -> (None, Some y) :: allign (x::xs) ys compareBy
                | _ -> []

        let inline zipZero xs ys zeroX zeroY =
            let rec loop xs ys =
                match (xs, ys) with
                | x::xs, y::ys -> (x, y) :: loop xs ys
                | [], ys -> List.zip (List.replicate ys.Length zeroX) ys
                | xs, [] -> List.zip xs (List.replicate xs.Length zeroY)
            loop xs ys

