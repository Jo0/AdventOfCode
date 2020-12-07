// Learn more about F# at http://fsharp.org

open System
open System.IO


let getSeatId(boardingPass:string) = 
    let rec getRow(boardingPass:string, range: int*int) =
        let midpoint = ((snd range) + (fst range))/2

        match boardingPass with
        | front when boardingPass.Length > 1 && boardingPass.Substring(0,1) = "F" ->
            getRow(boardingPass.Substring(1), (fst range, midpoint))
        | back when boardingPass.Length > 1 && boardingPass.Substring(0,1) = "B"  ->
            getRow(boardingPass.Substring(1), (midpoint+1, snd range))
        | finalFront when boardingPass.Length = 1 && boardingPass = "F" ->
            match finalFront with 
            | getRange when not (snd range - fst range = 1) ->
                getRow(finalFront, (fst range, midpoint))
            | _ -> 
                fst range
        | finalBack when boardingPass.Length = 1 && boardingPass = "B" ->
            match finalBack with 
            | getRange when not (snd range - fst range = 1) ->
                getRow(finalBack, (midpoint+1, snd range))
            | _ -> 
                snd range
        | _ -> raise <| new InvalidDataException($"Unknown character in {boardingPass}")

    let rec getCol(boardingPass:string, range: int*int) =
        let midpoint = ((snd range) + (fst range))/2
        match boardingPass with
        | left when boardingPass.Length > 1 && boardingPass.Substring(0,1) = "L"  ->
            getCol(boardingPass.Substring(1), (fst range, midpoint))
        | right when boardingPass.Length > 1 && boardingPass.Substring(0,1) = "R" ->
            getCol(boardingPass.Substring(1), (midpoint+1, snd range))
        | finalLeft when boardingPass.Length = 1 && boardingPass = "L" ->
            match finalLeft with 
            | getRange when not (snd range - fst range = 1) ->
                getCol(finalLeft, (fst range, midpoint))
            | _ -> 
                fst range
        | finalRight when boardingPass.Length = 1 && boardingPass = "R" ->
            match finalRight with 
            | getRange when not (snd range - fst range = 1) ->
                getCol(finalRight, (midpoint+1, snd range))
            | _ -> 
                snd range
        | _ -> raise <| new InvalidDataException($"Unknown character in {boardingPass}")

    let rowString = boardingPass.Substring(0,7)
    let columnString = boardingPass.Substring(7,3)
    let row = getRow(rowString,(0,127))
    let col = getCol(columnString, (0,7))
    (row * 8)  + col

[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")

    let seatIds = input |> Seq.toList
                        |> List.map(fun x -> getSeatId(x))
                        
    let sortedIds =  seatIds |> List.sort
    let mySeat = (seatIds |> List.where(fun x -> (not (List.contains(x+1) seatIds)) && (List.contains(x+2) seatIds)) |> List.head) + 1
                 
    printf "\n\n\n\n\n\n"
    printf "%d\n" (List.max seatIds)
    printf "%d\n" (mySeat) 
    printf "%d\n" (getSeatId("FBFBBFFRLR"))
    printf "%d\n" (getSeatId("BFFFBBFRRR"))
    printf "%d\n" (getSeatId("FFFBBBFRRR"))
    printf "%d\n" (getSeatId("BBFFBBFRLL"))
    printf "%d\n" (getSeatId("BFFFFBFLRL"))
    printf "%d\n" (getSeatId("BFFFFBBLRL"))

    0 // return an integer exit code

