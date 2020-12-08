// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Collections.Generic

type Content = {
    Name: string
    Quantity: int
}

type Bag = {
    Name: string
    Contents: Content list
}



let mapLuggageRules(input: string list) = 
    let cleanLine(line: string) =
        ((line.Trim('.').Replace(" bags","").Replace(" bag", "")).Split(" contain "))
    
    let mapBagContents(contentPart:string) = 
        let rec contents(content: string list) =
            match content with 
            | [] -> []
            | noOther when noOther.Head = "no other" -> []
            | x :: xs -> 
                let quantity = x.Substring(0, (x.IndexOf(' ') + 1))
                let bag = x.Substring(x.IndexOf(' ')+1)
                [{Name = bag; Quantity = (Int32.Parse quantity)}] @ contents(xs)

        contents((contentPart.Split(", ") |> Seq.toList))
    
    let mapBags(input: string list) = 
        let rec processBags(input: string list)= 
            match input with 
            | [] -> []
            | x :: xs -> 
                let lineParts = cleanLine(x)
                let contents  = mapBagContents(lineParts.[1])
                [{Name=lineParts.[0]; Contents = contents}] @ processBags(xs)
     
        processBags(input)
    
    mapBags(input)

let findBagsThatCanHold(bagName: string, luggageRules: Bag list) =
    let directlyHoldBag = luggageRules |> List.where(fun x -> x.Contents |> List.exists(fun y -> y.Name = bagName))

    let rec findBagsThatHoldDirect(bagsThatDirectlyHold: Bag list) =
        match bagsThatDirectlyHold with
        | [] -> []
        | x :: xs -> 
           let containsThisBag = (luggageRules |> List.where(fun x1 -> x1.Contents |> List.exists(fun y -> y.Name = x.Name)))
           [x] @ findBagsThatHoldDirect(containsThisBag) @ findBagsThatHoldDirect(xs)
    
    findBagsThatHoldDirect(directlyHoldBag) |> List.distinctBy(fun x -> x.Name)
    
let findContentsOfBag(bagName: string, luggageRules: Bag list) =
    let bag = luggageRules |> List.where(fun x -> x.Name = bagName) |> List.head

    let rec findBagContentQuantity(contents: Content list) =
        match contents with
        | [] -> 1
        | x :: xs -> 
           let bag = (luggageRules |> List.where(fun x1 -> x1.Name = x.Name)) |> List.head
           let bagQuantity = findBagContentQuantity(bag.Contents)
           let nextBagQuantity = findBagContentQuantity(xs)
           let quantity = x.Quantity
           (quantity *  bagQuantity) +  nextBagQuantity
    
    findBagContentQuantity(bag.Contents) - 1 
    
[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")

    let luggageRules = input |> Seq.toList
                             |> mapLuggageRules
    
    let canHoldShinyGold = findBagsThatCanHold("shiny gold", luggageRules)
 
    let shinyGoldHolds = findContentsOfBag("shiny gold", luggageRules)
    0 // return an integer exit code
