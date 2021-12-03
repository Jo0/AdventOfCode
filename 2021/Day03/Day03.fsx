open System
open System.IO

let input = File.ReadAllLines("input.txt") |> Array.toList

type BitCount = {
    Zero: int;
    One: int;
}

let explode(s:string) =
    ([for c in s -> c])

let rec transpose = function 
    | (_::_)::_ as M -> List.map List.head M :: transpose (List.map List.tail M)
    | _ -> []

let getBitCount(bits:List<char>) =
    let bitCount = {Zero = 0; One = 0;}

    let rec countBits(bits:List<char>, bitCount:BitCount) =
        match bits with 
        | [] -> bitCount
        | head :: tail -> 
            match head with 
            | '0' -> countBits(tail, {bitCount with Zero = bitCount.Zero + 1})
            | '1' -> countBits(tail, {bitCount with One = bitCount.One + 1})
            | _ -> countBits(tail, bitCount)
   
    countBits(bits, bitCount)

let getBitCountOfNumbers(numbers:List<string>) =
    numbers |> List.map(fun x -> explode(x)) 
            |> transpose  
            |> List.map(fun x -> getBitCount(x)) 

let getCommonBit(bitCount:BitCount) =
    if bitCount.Zero > bitCount.One then 
        '0'
    else
        '1'

let getLeastCommonBit(bitCount:BitCount) =
    if bitCount.Zero < bitCount.One then 
        '0'
    else
        '1'

let getGammaBinary(bitCounts:List<BitCount>) =
    let rec buildGamma(bitCounts:List<BitCount>, binaryString:string) =
        match bitCounts with 
        | [] -> binaryString
        | head :: tail ->
            buildGamma(tail, binaryString + (string (getCommonBit(head))))

    buildGamma(bitCounts,"")
    
let getEpsilonBinary(bitCounts:List<BitCount>) =
    let rec buildEpsilon(bitCounts:List<BitCount>, binaryString:string) =
        match bitCounts with 
        | [] -> binaryString
        | head :: tail ->
            buildEpsilon(tail, binaryString + (string (getLeastCommonBit(head))))

    buildEpsilon(bitCounts,"")

let binaryStringToDecimal(binaryString:string) =
    Convert.ToInt32(binaryString, 2)

let bitCounts = getBitCountOfNumbers(input)

let gammaRate = bitCounts |> getGammaBinary |> binaryStringToDecimal

let epsilonRate = bitCounts |> getEpsilonBinary |> binaryStringToDecimal

printfn "Part 1 : %d" (gammaRate * epsilonRate)


let getOxygenRate(numbers: List<string>) =
    let getBitToTake(bitCount:BitCount) =
        if bitCount.Zero = bitCount.One then
            '1'
        else 
            getCommonBit(bitCount)

    let filterNumbers(position:int, numbers:List<string>) =
        let bit  = numbers |> getBitCountOfNumbers |> List.skip (position) |> List.head |> getBitToTake
        let filtered = numbers |> List.where(fun x -> x.[position] = bit)
        if filtered.Length > 0 then 
            filtered
        else
            numbers
    
    let rec oxygenRate(position:int, numbers:List<string>) =
        if position = numbers.Head.Length then 
            numbers.Head
        else 
            oxygenRate(position+1, filterNumbers(position, numbers))
    
    oxygenRate(0, numbers)
    

let getCo2ScrubberRating(numbers: List<string>) =
    let getBitToTake(bitCount:BitCount) =
        if bitCount.Zero = bitCount.One then
            '0'
        else 
            getLeastCommonBit(bitCount)

    let filterNumbers(position:int, numbers:List<string>) =
        let bit  = numbers |> getBitCountOfNumbers |> List.skip (position) |> List.head |> getBitToTake
        let filtered = numbers |> List.where(fun x -> x.[position] = bit)
        if filtered.Length > 0 then 
            filtered
        else
            numbers
    
    let rec co2Rate(position:int, numbers:List<string>) =
        if position = numbers.Head.Length then 
            numbers.Head
        else 
            co2Rate(position+1, filterNumbers(position, numbers))
    
    co2Rate(0, numbers)
    
let oxygenRate = getOxygenRate(input) |> binaryStringToDecimal
let co2Rate = getCo2ScrubberRating(input) |> binaryStringToDecimal

printfn "Part 2 : %d" (oxygenRate * co2Rate) 