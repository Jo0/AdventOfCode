open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.toList

type Entry = {
    Signal:string;
    Output:string;
} with
    member this.allSignals = (this.Signal + " " + this.Output)

type Digit = {
    Number:string;
    Signal: string;
}

let toEntry (line:string) =
    {Signal = line.Split(" | ").[0]; Output = line.Split(" | ").[1]}

let isEasyDigit (pattern: string) =
    match pattern with
    | one when (pattern.Length = 2) -> true
    | four when (pattern.Length = 4) -> true
    | seven when (pattern.Length = 3) -> true
    | eight when (pattern.Length = 7) -> true
    | _ -> false

let findEasyDigitsInOutput (entry:Entry) =
    entry.Output.Split(" ") |> Array.where isEasyDigit 
                            |> Array.toList 
                            |> List.sortBy (fun x -> x.Length)

let findEasyDigits (entry: Entry) =
    entry.allSignals.Split(" ") |> Array.where isEasyDigit 
                                  |> Array.toList 
                                  |> List.sortBy (fun x -> x.Length)

let getNumberSignal (number:string) (maps:List<Digit>) =
    maps |> List.where(fun x -> x.Number = number) |> List.head |> (fun x -> x.Signal)

let getSignalNumber (maps:List<Digit>) (signal:string)  =
    maps |> List.where(fun x -> x.Signal = signal) |> List.head |> (fun x -> x.Number)

let matchingCharCount (s1:string) (s2:string) =
    let count = s1 |> Seq.map (fun x -> s2 |> Seq.where(fun c -> c = x) |> Seq.length) |> Seq.sum
    count 

let rec toEasyDigits (signals: List<string>) =
    let toDigit (signal:string) = 
        match signal with
        | one when (signal.Length = 2) -> {Number = "1"; Signal = one;}
        | four when (signal.Length = 4) -> {Number = "4"; Signal = four;}
        | seven when (signal.Length = 3) -> {Number = "7"; Signal = seven;}
        | eight when (signal.Length = 7) -> {Number = "8"; Signal = eight;}

    match signals with 
    | [] -> []
    | head :: tail -> 
        [toDigit head] @ toEasyDigits tail

let decodeOutputs (entries:List<Entry>) =
    let determineNumber (maps:List<Digit>) (signal:string)  =
        match signal with 
        | threefiveortwo when (signal.Length = 5) ->
            match threefiveortwo with
            | three when ((matchingCharCount three (getNumberSignal "1" maps)) = 2) -> {Number = "3"; Signal = three;}
            | five when ((matchingCharCount five (getNumberSignal "4" maps)) = 3) -> {Number = "5"; Signal = five;}
            | two -> {Number = "2"; Signal = two;}
        | sixnineorzero when (signal.Length = 6) -> 
            match sixnineorzero with 
            | six when ((matchingCharCount six (getNumberSignal "1" maps)) = 1) -> {Number = "6"; Signal = six;}
            | nine when ((matchingCharCount nine (getNumberSignal "4" maps)) = 4) -> {Number = "9"; Signal = nine;}
            | zero -> {Number = "0"; Signal = zero;}
        | s -> {Number = "NaN"; Signal = s;}

    let rec getMap (entry: Entry) =
        let easyMap = entry |> findEasyDigits
                            |> toEasyDigits

        let entryMaps = entry.allSignals.Split(" ") |> Array.toList
                                                    |> List.sortBy (fun x -> x.Length)
                                                    |> List.distinct
                                                    |> List.where(fun x -> (easyMap |> List.where(fun y -> y.Signal.Equals(x))).Length = 0) 
                                                    |> List.map (fun x -> determineNumber easyMap x)

        easyMap @ entryMaps

    let decodeEntry (entry:Entry) = 
        let map = getMap entry

        let decodeSignals (map:List<Digit>) (entry:Entry) =
            (entry.Output.Split(" ")) |> Array.map(fun x -> (getSignalNumber map x)) |> Array.toSeq |> String.Concat |> Int32.Parse

        decodeSignals map entry

    entries |> List.map decodeEntry

let entries = input |> List.map toEntry

let easyDigitsCount = entries |> List.map findEasyDigitsInOutput
                              |> List.sumBy (fun x -> x.Length)

printfn "%A" easyDigitsCount

let totalOutput = entries |> decodeOutputs 
                          |> List.sum

printfn "%A" totalOutput
