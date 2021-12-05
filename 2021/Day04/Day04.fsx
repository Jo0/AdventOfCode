open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.toList

type Position = 
    | Unmarked of int
    | Marked of int

type Card = {
    Positions:List<List<Position>>;
}

let loadPositions(input:List<string>) =
    let rec parsePositions(input:List<string>, cards:List<Card>) =
        match input with
        | [] -> cards
        | head :: tail ->
            if (head.Equals("\"") || head.Length = 0) then 
                parsePositions(tail,cards)
            else
               let rest = [head] @ (tail |> List.take 4)
               let positions = rest |> List.map(fun x -> x.Split(" ") |> Array.where(fun x -> not (x.Equals(""))) |> Array.map(fun x ->  Unmarked (Int32.Parse(x.ToString())) ) |> Array.toList)

               parsePositions((tail |> List.skip 4), cards @ [{Positions = positions;}])

    let cards:List<Card> = []

    parsePositions(input, cards)

let getCardColumns(position:List<List<Position>>) =
    let rec transpose = function 
    | (_::_)::_ as M -> List.map List.head M :: transpose (List.map List.tail M)
    | _ -> []

    transpose position |> List.map(fun x -> x |> List.map(fun x -> x))

let markCard(number: int, card:Card) =
    let positions = card.Positions |> List.map(fun x -> x |> List.map(fun x -> if x = Unmarked number then Marked number else x))
    {card with Positions = positions}

let isMarked c =
    match c with
    | Unmarked _ -> false
    | Marked _ -> true

let hasWon(position:List<List<Position>>) =
    let row = position |> List.tryFind(fun r -> r |> List.filter (isMarked >> not) |> List.length = 0)
                       |> Option.isSome

    let column = getCardColumns(position) |> List.tryFind(fun r -> r |> List.filter (isMarked >> not) |> List.length = 0)
                                          |> Option.isSome

    row || column

let hasCardWon(card:Card) =
    let row = card.Positions |> List.tryFind(fun r -> r |> List.filter (isMarked >> not) |> List.length = 0)
                       |> Option.isSome

    let column = getCardColumns(card.Positions) |> List.tryFind(fun r -> r |> List.filter (isMarked >> not) |> List.length = 0)
                                          |> Option.isSome

    row || column

let getPositionValue(position:Position) =
    match position with
    | Unmarked v -> v
    | Marked v -> v

let score(winningDraw:int, card:Card) =
    let sum = card.Positions |> List.collect(fun x -> x)
                             |> List.filter(isMarked >> not)
                             |> List.map getPositionValue
                             |> List.sum
    sum * winningDraw

let rec play1(draws:List<int>, cards:List<Card>) =
    let draw = draws |> List.head
    let rest = draws |> List.tail

    let markedBoards = cards |> List.map(fun x -> markCard(draw, x))

    let winner = markedBoards |> List.tryFind(fun x -> hasCardWon(x))
    
    match winner with 
    | None -> play1(rest, markedBoards)
    | Some b -> (draw, b)

    
let draws = input.Head.Split(",") |> Seq.map(fun x -> int x) |> Seq.toList

let cards = input |> List.skip 1 |> loadPositions

let winningDraw, card = play1(draws,cards)

printfn "%d" winningDraw
printfn "%A" card.Positions
printfn "%d" (score(winningDraw,card))


let rec play2(draws:List<int>, cards:List<Card>) =
    let draw = draws |> List.head
    let rest = draws |> List.tail

    let markedBoards = cards |> List.map(fun x -> markCard(draw, x)) |> List.filter (hasCardWon >> not)
    
    match markedBoards with 
    | [] -> draw, (markCard(draw, cards.[0]))
    | _ -> play2(rest, markedBoards)

let lastwinningDraw, lastcard = play2(draws,cards)

printfn "%d" lastwinningDraw
printfn "%A" lastcard.Positions
printfn "%d" (score(lastwinningDraw,lastcard))
