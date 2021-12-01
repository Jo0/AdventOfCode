open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.map(fun x -> int x) |> Array.toList

let rec increasedCount (list:List<int>) =
    match list with
    | [] -> 0
    | [ _ ] -> 0
    | head :: tail -> 
        if head < tail.Head then 
            1 + (increasedCount tail)
        else 
            increasedCount tail

let increasedCountWithSlidingWindow(list:List<int>, windowSize:int) =
    let rec buildSlidingWindow (list:List<int>, size:int) =
        match list with 
        | [] -> []
        | sizeOfWindow when (list.Length = windowSize) -> 
            [(sizeOfWindow |> List.sum)]
        | head :: tail -> 
            let window = head + (tail |> List.take (size-1) |> List.sum)
            [ window ] @ buildSlidingWindow(tail, size)
    
    increasedCount (buildSlidingWindow(list,windowSize))


printfn $"Increased count: {increasedCount input}" //1451
printfn $"Increased count of sliding window size 3: {increasedCountWithSlidingWindow(input, 3)}" //1395
