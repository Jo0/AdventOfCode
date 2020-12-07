// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions

let groupAnswers(input:string list) = 
    
    let getGroupAnswers(input:string list) =

        let rec getGroupAnswers(input:string list) =
            match input with 
            | [] -> ""
            | x :: xs -> 
                match x with 
                | "" -> "; " + getGroupAnswers(xs)
                | _ -> x + getGroupAnswers(xs)
    
        let groupAnswers = getGroupAnswers(input)

        groupAnswers.Split("; ") |> Seq.toList

    let getGroupQuestions(groupAnswers:string) =
        let questions = new Dictionary<string, int>()
        let rec mapAnswersToQuestions(groupAnswers:string, questionsMap: Dictionary<string, int>) =
            match groupAnswers with 
            | "" -> questionsMap
            | answer when Regex.IsMatch(groupAnswers.Substring(0,1), "[a-z]") -> 
                let question = answer.Substring(0,1)
                if not (questionsMap.ContainsKey(question)) then questionsMap.Add(question, 1) else questionsMap.[question] <- questionsMap.[question] + 1
                mapAnswersToQuestions(groupAnswers.Substring(1), questionsMap)
            | _ -> raise <| Exception $"Not valid question {groupAnswers}"

        mapAnswersToQuestions(groupAnswers, questions)

    let groupAnswers = input |> getGroupAnswers                            
                             |> List.map(fun x -> getGroupQuestions(x))

    groupAnswers

let groupAnsweredAllQuestions(groupsAnswers: Dictionary<string,int>) =
    if not (groupsAnswers.Keys.Count = 26) then
        false
    else
        let anAnswer = groupsAnswers.Values |> Seq.toList |> List.head
        groupsAnswers.Values |> Seq.toList
                             |> List.forall(fun x -> x = anAnswer)  

let getGroupAnswerByPeople(input: string list) =

    let getGroupAnswers(input:string list) =

        let rec getGroupAnswers(input:string list) =
            match input with 
            | [] -> ""
            | x :: xs -> 
                match x with 
                | "" -> "; " + getGroupAnswers(xs)
                | _ -> x + "," + getGroupAnswers(xs)

        let groupAnswers = getGroupAnswers(input).Split("; ") |> Seq.map(fun x -> x.TrimEnd(','))

        groupAnswers |> Seq.toList

    let getGroupQuestions(groupAnswers:string) =
        let questions = new Dictionary<string, int>()
        let rec mapAnswersToQuestions(groupAnswers:string, questionsMap: Dictionary<string, int>) =
            match groupAnswers with 
            | "" -> questionsMap
            | answer when Regex.IsMatch(groupAnswers.Substring(0,1), "[a-z]") -> 
                let question = answer.Substring(0,1)
                if not (questionsMap.ContainsKey(question)) then questionsMap.Add(question, 1) else questionsMap.[question] <- questionsMap.[question] + 1
                mapAnswersToQuestions(groupAnswers.Substring(1), questionsMap)
            | _ -> raise <| Exception $"Not valid question {groupAnswers}"

        mapAnswersToQuestions(groupAnswers.Replace(",",""), questions)

    let groupAnswers = input |> getGroupAnswers
                             |> List.map(fun x -> (x.Split(",").Length, getGroupQuestions(x)))
                             
    groupAnswers
                           

[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")

    let groupAnswersCount = input |> Seq.toList
                                  |> groupAnswers
                                  |> List.sumBy(fun x -> x.Keys.Count)
                             
    let groupAnswersCount2 = input |> Seq.toList
                                   |> getGroupAnswerByPeople
                                   |> List.sumBy(fun x -> 
                                                          let numOfPeople = (fst x)
                                                          let numberOfQuestionsAnsweredByAllPeople = (snd x).Values |> Seq.where(fun v -> v = numOfPeople) |> Seq.length
                                                          numberOfQuestionsAnsweredByAllPeople
                                                )

    printf "%d" groupAnswersCount
    printf "%d" groupAnswersCount2

    0 // return an integer exit code
