// Learn more about F# at http://fsharp.org

open System
open System.IO

type Password = {
    occurence: int * int
    character: string
    password: string
}

let parsePolicy(line:string) =
    let parts = line.Split(' ')
    let occurence = parts.[0].Split('-') |> Seq.toList
    let occurenceTuple = 
        match occurence with
        | [_; _] -> (Int32.Parse occurence.[0], Int32.Parse occurence.[1])
        | _ -> (0,0)
    let character = parts.[1].Replace(":", "")
    let input = parts.[2]
    
    let password : Password = {occurence = occurenceTuple; character = character; password = input}
    password

let rec parsePolicies list =
    match list with 
    | [] -> []
    | x :: xs -> 
        let policy = [parsePolicy x]
        policy @ parsePolicies xs

let rec characterOccurenceCount(password:string, char:string) = 
    match password with
    | "" -> 0
    | s -> 
        let tail = s.Substring(1)
        let head = s.Substring(0,1)
        match head with
        |  same when head = char -> 1 + characterOccurenceCount(tail, char) 
        | _ -> 0 + characterOccurenceCount(tail, char)
      
let validPassword(password:Password) =
    match password with
    | {occurence = (0,0); character = _; password = _} -> false
    | {occurence = (low, high); character = c; password = p} -> 
        let charCount = characterOccurenceCount(p,c)
        charCount >= low && charCount <= high

let validPassword2(password:Password) =
    match password with
       | {occurence = (0,0); character = _; password = _} -> false
       | {occurence = (pos1, pos2); character = c; password = p} -> 
           let char = c.ToCharArray().[0]
           (p.[pos1-1] = char) <> (p.[pos2-1] = char)

[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")
    
    let validPasswords = input |> Seq.toList
                               |> parsePolicies
                               |> List.where(fun x -> validPassword x) 
    printf "%d" validPasswords.Length
    
    printf "\n"

    let validPasswords2 = input |> Seq.toList
                                |> parsePolicies
                                |> List.where(fun x -> validPassword2 x) 
    printf "%d" validPasswords2.Length

    0 // return an integer exit code
