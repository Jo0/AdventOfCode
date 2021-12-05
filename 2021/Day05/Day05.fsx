open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.toList

type Line = {
    Start:int*int;
    End:int*int;
} with 
    member this.isVertical = fst(this.Start) = fst(this.End)
    member this.isHorizonal = snd(this.Start) = snd(this.End)
    member this.isHorizonalOrVertical = this.isHorizonal || this.isVertical
    member this.dy = snd(this.End) - snd(this.Start)
    member this.dx = fst(this.End) - fst(this.Start)
    member this.dydx = (this.dy, this.dx)
    member this.orderHorizonalAndVertical = 
        if this.isVertical && (snd(this.End) < snd(this.Start)) then 
            {Start = this.End; End = this.Start} 
        elif this.isHorizonal && (fst(this.End) < fst(this.Start)) then 
            {Start = this.End; End = this.Start} 
        else this

type Point = int * int 

let stringPairToIntPair(pairString:string[]) =
    pairString |> Array.map(fun a -> a.Split(",")) |> Array.map(fun x -> (Int32.Parse(x.[0]), Int32.Parse(x.[1]))) |> Array.toList

let mapPoints(lines:List<Line>) = 
    let getPoints(line:Line) =
        let mutable points:List<Point> = []
        let (x1, y1) = line.Start
        let (x2, y2) = line.End

        if line.isVertical then 
            for i = y1 to y2 do
                points <- points @ [Point (x1, i)]
        elif line.isHorizonal then
            for i = x1 to x2 do
                points <- points @ [Point (i, y1)]
        else
            let (dy, dx) = line.dydx
            let mutable x = 0;
            let mutable y = 0;
            x <- x1
            y <- y1
            match (dy,dx) with 
            | downRight when dx > 0 && dy > 0 ->
                while (x <= x2 && y <= y2) do
                    points <- points @ [Point (x, y)]
                    x <- x + 1
                    y <- y + 1
            | downLeft when dx > 0 && dy < 0 ->
                while (x <= x2 && y >= y2) do
                    points <- points @ [Point (x, y)]
                    x <- x + 1
                    y <- y - 1
            | upRight when  dx < 0 && dy > 0 ->
                while (x >= x2 && y <= y2) do
                    points <- points @ [Point (x, y)]
                    x <- x - 1
                    y <- y + 1
            | upLeft when dx < 0 && dy < 0 ->
                while (x >= x2 && y >= y2) do
                    points <- points @ [Point (x, y)]
                    x <- x - 1
                    y <- y - 1
            | _ -> points <- points
        
        points

    let points = lines |> List.map(fun x -> getPoints(x)) |> List.reduce List.append
    points |> List.groupBy(fun x -> x) |> List.map(fun x -> (fst(x), snd(x).Length))
    

let lines = input |> List.map(fun x -> x.Split(" -> "))
                  |> List.map(stringPairToIntPair)
                  |> List.map(fun x -> {Start = x.[0]; End = x.[1]})

let horizontalOrVerticalLines = lines |> List.filter(fun x -> x.isHorizonalOrVertical)
                                      |> List.map(fun x -> x.orderHorizonalAndVertical) 
                                      |> mapPoints 
                                      |> List.where(fun x -> snd(x) >= 2)
                                      |> List.length

let allLines = lines |> List.map(fun x -> x.orderHorizonalAndVertical) 
                     |> mapPoints 
                     |> List.where(fun x -> snd(x) >= 2)
                     |> List.length


printfn "%A" horizontalOrVerticalLines
printfn "%A" allLines
