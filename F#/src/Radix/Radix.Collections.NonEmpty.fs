namespace Radix.Collections

type NonEmpty<'a> = 
| Singleton of 'a
| List of Head : 'a * Tail : 'a list

module NonEmpty =

    let inline toList x = 
        match x with
        | Singleton x -> [x]
        | List (head, tail) -> head :: tail

    let head x = 
        match x with
        | Singleton x -> x
        | List (head, _) -> head   

    let tail x = 
        match x with
        | Singleton x -> [x]
        | List (_, tail) -> tail      

    let create x xs = List(x, xs)

    let inline ofList xs =
        List(List.head xs, List.tail xs)    

    let inline length (x: NonEmpty<'a>) = 
        match x with
        | Singleton _ -> 1
        | List (_, xs) -> 1 + List.length xs

    let inline append (x: NonEmpty<'a>) (y: NonEmpty<'a>) = 
        match x, y with
        | Singleton x, Singleton y -> List (x, [y])
        | Singleton x, List (y, ys) -> List (x, y::ys)
        | List (x, xs), Singleton y -> List (x, y::xs |> List.rev)
        | List (x, xs), List(y, ys) -> List (x, y::xs |> List.rev |> List.append ys)

    let inline map (projection: 'a -> 'b) (nonEmpty: NonEmpty<'a>) : NonEmpty<'b> =
        match nonEmpty with
        | Singleton x -> (Singleton (projection x))
        | List (x, xs) -> 
            let list = (projection x) :: List.map projection xs 
            List(List.head list, List.tail list)

    let inline bind (nonEmpty: NonEmpty<'a>) (f: 'a -> NonEmpty<'b>) : NonEmpty<'b> =
        match nonEmpty with
        | Singleton x -> f x
        | List (x, xs) -> xs |> List.map f |> List.fold append (f x)        

    let inline combine< ^a, ^b> (f: 'a-> 'a-> 'b) (x: NonEmpty<'a>) (y: NonEmpty<'a>) =  
        match x, y with
        | Singleton x', Singleton y' -> Singleton (f x' y')
        | Singleton x', List (_, __) -> y |> map (fun y' -> f x' y') 
        | List (_, __), Singleton y' -> x |> map (fun x' -> f x' y') 
        | List (x, xs), List(y, ys) -> List(f x y, List.map2 f xs ys)

    let inline (@) (x: NonEmpty<'a>) (y: NonEmpty<'a>) = append x y   
    let inline filter predicate (x: NonEmpty<'a>) : NonEmpty<'a> =
        match x with
        | Singleton x' when predicate x' -> x
        | List (x, xs) -> 
            let matches = x::xs |> List.filter predicate
            List(List.head matches, List.tail matches)

    let inline fold folder (state:'a) (list: NonEmpty<'b>) = 
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(folder)
        let mutable acc = state        
        match list with 
        | Singleton x -> 
            acc <- f.Invoke(state, x)
            acc
        | List (x, xs) -> 
            let mutable acc = state
            for x' in x::xs do
                acc <- f.Invoke(acc, x')
            acc

    let inline sumBy (projection: 'a -> 'b) (list: NonEmpty<_>) =
        list |> map projection |> fold (+) (list |> head)   

    let inline sum (list: NonEmpty<'a>) = sumBy id list    

    let inline maxBy (projection) (list: NonEmpty<_>) =
        list |> fold (fun x y -> if projection x > projection y then x else y) (list |> head)  

    let inline max (list: NonEmpty<'a>) = maxBy id list        
   
    type NonEmptyMonadBuilder() =
        member inline __.Bind (r, f) = bind r f
        member inline __.Return x = Singleton x
        member __.ReturnFrom m = m

        member __.Zero() = Singleton ()

        member inline __.For (r, f) = bind r f

    let nonEmpty = NonEmptyMonadBuilder()                
                          