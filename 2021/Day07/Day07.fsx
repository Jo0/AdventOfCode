open System
open System.IO

let input = (File.ReadAllLines("input.txt") |> Array.head).Split(',') |> Array.map(fun x -> Int32.Parse(x)) |> Array.toList

let getHorizontalPositionCounts (crabs: List<int * List<int>>) =
    let getHorizontalPositionCount (crab: int * List<int>) =
        let horizontal, crabs = crab
        (horizontal, crabs.Length)

    crabs |> List.map getHorizontalPositionCount
    
let calculateFuelPerHorizontalAlignment (horizontals: List<int * int>) =
    let fuelWhenAlignedWith (horizontals: List<int * int>) (horizontal:int * int) =
        let rec calculateFuelForSteps(steps:int) =
            match steps with 
            | 0 -> 0
            | s -> calculateFuelForSteps(s-1) + s

        let h, _ = horizontal

        let fuel = horizontals |> List.map (fun x -> 
                                                let h', count = x
                                                let fuelCost = calculateFuelForSteps (Math.Abs(h'-h))
                                                fuelCost * count
                                            )
                                |> List.sum
        
        (h, fuel)
        
    horizontals |> List.map (fuelWhenAlignedWith horizontals)

let crabs = input |> List.groupBy(fun x -> x) 
                  |> getHorizontalPositionCounts

printfn "%A" crabs

let fuel = crabs |> calculateFuelPerHorizontalAlignment

printfn "%A" fuel

printfn "%A" (fuel |> List.minBy(fun x -> snd(x)))