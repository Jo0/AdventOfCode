open System
open System.IO

let input = (File.ReadAllLines("input.txt") |> Array.head).Split(',') |> Array.map(fun x -> Int32.Parse(x)) |> Array.toList

let getFishAfterDays1(fish:List<int>, days:int) =
    let updateTimer(timer:int) =
        match timer with
        | 0 -> 6
        | _ -> timer - 1

    let rec simulate(fish:List<int>, day:int) =
        let newFish = [ for i in 1 .. (fish |> List.where(fun x -> x = 0) |> List.length) -> 8 ]
        let updatedFish = (fish |> List.map(updateTimer)) @ newFish
        //printfn "Day %d: %A" day updatedFish

        match day with
        | final when day = days -> 
            updatedFish
        | _ -> 
            simulate(updatedFish, day + 1)

    simulate(fish,1)

let getFishAfterDays2(fish:List<int>, days:int) =
    let loadTimers(fish:List<int>) =
        let fishTimers = fish |> List.groupBy(fun x -> x) |> List.map(fun x -> (fst(x),(snd(x) |> List.length)))
        let timers:List<Int64> = [ for i in 0 .. 8 -> 0]
        
        let rec setTimers(fishTimers:List<int*int>, timers:List<Int64>) =
            match fishTimers with 
            | [] -> timers 
            | head :: tail -> 
                let t, c = head
                let updatedTimers:List<Int64> = timers |> List.mapi(fun i x -> if i = t then c else x)
                setTimers(tail, updatedTimers)
        
        setTimers(fishTimers, timers)

    let timers = loadTimers(fish)
    
    let updateTimers(timers:List<Int64>) =
        let resetting = timers.[0]
        timers |> List.mapi(fun i x -> if i < 8 then timers.[(i+1)] else x) 
               |> List.mapi(fun i x -> if i = 6 then x + resetting else x)
               |> List.mapi(fun i x -> if i = 8 then resetting else x) 

    let rec simulate(timers:List<Int64>, day:int) =
        let updatedTimers = updateTimers(timers)
        //printfn "Day %d: %A" day updatedTimers
        match day with
        | final when day = days -> 
            updatedTimers
        | _ -> 
            simulate(updatedTimers, day + 1)

    simulate(timers,1)

getFishAfterDays1(input,80) |> List.length |> printfn "%d"
getFishAfterDays2(input, 256) |> List.sum |> printfn "%d"
