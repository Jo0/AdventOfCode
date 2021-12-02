open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.toList

type Position = {
    Horizontal:int;
    Depth:int;
    Aim: int;
}

let splitInput(input:string) = 
    input.Split(' ').[0], (Int32.Parse(input.Split(' ').[1]))

let rec processInputs(list:List<string>, position:Position) = 
    match list with
    | [] -> position
    | head :: tail ->
        let input = splitInput head
        match input with 
        | ("forward", unit) -> processInputs(tail, {position with Horizontal = position.Horizontal + unit})
        | ("down", unit) -> processInputs(tail, {position with Depth = position.Depth + unit})
        | ("up", unit) -> processInputs(tail, {position with Depth = position.Depth - unit})
        | ( _, _) -> position

let rec processInputsWithAim(list:List<string>, position:Position) =
    match list with
    | [] -> position
    | head :: tail ->
        let input = splitInput head
        match input with 
        | ("forward", unit) -> processInputsWithAim(tail, {position with Horizontal = position.Horizontal + unit; Depth = position.Depth + (position.Aim * unit)})
        | ("down", unit) -> processInputsWithAim(tail, {position with Aim = position.Aim + unit})
        | ("up", unit) -> processInputsWithAim(tail, {position with Aim = position.Aim - unit})
        | ( _, _) -> position

let positionAnswer(position:Position) = 
    position.Horizontal * position.Depth

let position = {Horizontal = 0; Depth = 0; Aim=0;}

let finalPosition = processInputs(input, position)
printfn $"{positionAnswer(finalPosition)}" // 1459206

let finalPositionWithAim = processInputsWithAim(input, position)
printfn $"{positionAnswer(finalPositionWithAim)}" // 1320534480
