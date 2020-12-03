// Learn more about F# at http://fsharp.org

open System
open System.IO


type SpaceState = 
| Open 
| Tree
| End

type Position = {
    X: int
    Y: int
}

type Space = {
    Position: Position
    State: SpaceState
}

type Dimension = {
    Length: int
    Height: int
}

type Geology = {
    Dimensions: Dimension
    Spaces: Space list
}

type Slope = {
    Right: int
    Down: int
}

let rec parseSpaces(height: int, length: int, line: string) =
    match line with 
    | "" -> []
    | s -> 
        let head = s.Substring(0, 1)
        let tail = s.Substring(1)

        let y = length - line.Length
        let position = {X = height; Y = y}

        match head with 
        | "." -> [{Position = position; State = Open}] @ parseSpaces(height, length, tail)
        | "#" -> [{Position = position; State = Tree}] @ parseSpaces(height, length, tail)
        | _ -> []


let rec mapSpaces(input: string list, rest: string list) =
    match rest with 
    | [] -> []
    | x :: xs -> 
        let height = List.findIndex (fun s -> s = x) input
        let length = x.Length

        let spaces = parseSpaces(height,length,x)
        spaces @ mapSpaces(input,xs)
                    
let mapGeology(input: string list) = 
    let dimensions = {Length = input.Head.Length; Height = input.Length;}
    { Dimensions = dimensions; Spaces = mapSpaces(input,input)}

let getNextSpace(dimensions: Dimension, slope: Slope, position: Position) =
    let moveDown = position.X + slope.Down
    let nextX = if moveDown >= dimensions.Height then -1
                else moveDown

    let moveRight = position.Y + slope.Right
    let nextY = if moveRight >= dimensions.Length then (moveRight - dimensions.Length) 
                else moveRight
    
    let nextSpace = {X = nextX; Y = nextY}
    nextSpace
                

let rec traverseGeologyCountingState(geology: Geology, slope: Slope, state: SpaceState, startingPos: Position) =
    let nextSpace = getNextSpace(geology.Dimensions, slope, startingPos)

    match nextSpace with 
    | {X = -1; Y = _} -> 0
    | {X = _; Y = _} -> 
        let space = geology.Spaces |> List.find (fun x -> x.Position.X = nextSpace.X && x.Position.Y = nextSpace.Y)
        match space.State with 
        | same when state = space.State -> (1 + traverseGeologyCountingState(geology, slope, state, nextSpace))
        | _ -> (0 + traverseGeologyCountingState(geology, slope, state, nextSpace))
             
    
[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")

    let geology = input |> Seq.toList 
                        |> mapGeology

    let slope = {Right = 3; Down = 1}
    let startingPosition = {X = 0; Y = 0}
    let treeCount = traverseGeologyCountingState(geology, slope, Tree, startingPosition)
    printf "%d" treeCount

    printf "\n"

    let slope1 = {Right = 1; Down = 1}
    let slope2 = {Right = 3; Down = 1}
    let slope3 = {Right = 5; Down = 1}
    let slope4 = {Right = 7; Down = 1}
    let slope5 = {Right = 1; Down = 2}

    let treeCount1 = traverseGeologyCountingState(geology, slope1, Tree, startingPosition)
    let treeCount2 = traverseGeologyCountingState(geology, slope2, Tree, startingPosition)
    let treeCount3 = traverseGeologyCountingState(geology, slope3, Tree, startingPosition)
    let treeCount4 = traverseGeologyCountingState(geology, slope4, Tree, startingPosition)
    let treeCount5 = traverseGeologyCountingState(geology, slope5, Tree, startingPosition)

    printf "%d" (treeCount1 * treeCount2 * treeCount3 * treeCount4 * treeCount5)



    0 // return an integer exit code
