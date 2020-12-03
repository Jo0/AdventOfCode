// Learn more about F# at http://fsharp.org

open System
open System.IO

let rec combos list = 
    match list with 
    | [] -> []
    | elm :: rest -> 
        let mapPairs = rest |> List.map (fun y -> (elm,y))
        mapPairs @ (combos rest)

let rec combos3 list = 
    match list with 
    | [] -> []
    | elm :: rest -> 
        let combos = combos rest
        let mapCombos = combos |> List.map (fun (y,z) -> (elm,y,z))
        mapCombos @ (combos3 rest)

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    let input = File.ReadAllLines("input.txt")
    
    input |> Seq.map int
          |> Seq.toList
          |> combos
          |> List.find(fun (x,y) -> x + y = 2020)
          |> fun (x,y) -> printf "%d" (x * y)

    printf "\n"
    input |> Seq.map int
          |> Seq.toList
          |> combos3
          |> List.find(fun (x,y,z) -> x + y + z = 2020)
          |> fun (x,y,z) -> printf "%d" (x * y * z)
    0 // return an integer exit code
