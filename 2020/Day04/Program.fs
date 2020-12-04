// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Text.RegularExpressions

type Passport = {
    BirthYear: string
    IssueYear: string
    ExpirationYear: string
    Height: string
    HairColor: string
    EyeColor: string
    PassportId: string
}

let passportContainsAllNeccessaryFields(passport:string) = 
    passport.Contains("byr") && passport.Contains("iyr") && passport.Contains("eyr") && passport.Contains("hgt") && passport.Contains("hcl") && passport.Contains("ecl") && passport.Contains("pid")


let passportMapper(passport:string) =
    let passportFields = passport.Split(' ') |> Seq.toList

    let birthYear = passportFields |> List.where(fun x -> x.StartsWith("byr:"))
                                   |> List.map(fun x -> x.Substring(4))
                                   |> List.head
    let issueYear = passportFields |> List.where(fun x -> x.StartsWith("iyr:"))
                                   |> List.map(fun x -> x.Substring(4))
                                   |> List.head
    let expirationYear = passportFields |> List.where(fun x -> x.StartsWith("eyr:"))
                                   |> List.map(fun x -> x.Substring(4))
                                   |> List.head
    let height = passportFields |> List.where(fun x -> x.StartsWith("hgt:"))
                                |> List.map(fun x -> x.Substring(4))
                                |> List.head                                   
    let hairColor = passportFields |> List.where(fun x -> x.StartsWith("hcl:"))
                                   |> List.map(fun x -> x.Substring(4))
                                   |> List.head
    let eyeColor = passportFields |> List.where(fun x -> x.StartsWith("ecl:"))
                                  |> List.map(fun x -> x.Substring(4))
                                  |> List.head
    let passportId = passportFields |> List.where(fun x -> x.StartsWith("pid:"))
                                    |> List.map(fun x -> x.Substring(4))
                                    |> List.head    

    {BirthYear = birthYear; IssueYear = issueYear; ExpirationYear = expirationYear; Height = height; HairColor = hairColor; EyeColor = eyeColor; PassportId = passportId}

let printPassport(passport:Passport) =
    passport.BirthYear + " " + passport.IssueYear + " " + passport.ExpirationYear + " " + passport.Height + " " + passport.HairColor + " " + passport.EyeColor + " " + passport.PassportId

let passportFieldsAreValid(passport:Passport) =
    let validBirthYear(birthYear:string) =
        let couldParse, birthYearVal = Int32.TryParse birthYear
        if couldParse then birthYearVal >= 1920 && birthYearVal <= 2002 else couldParse

    let validIssueYear(issueYear:string) =
         let couldParse, issueYearVal = Int32.TryParse issueYear
         if couldParse then issueYearVal >= 2010 && issueYearVal <= 2020 else couldParse
    
    let validExpirationYear(expirationYear:string) = 
        let couldParse, expirationYearVal = Int32.TryParse expirationYear
        if couldParse then expirationYearVal >= 2020 && expirationYearVal <= 2030 else couldParse
    
    let validHeight(height:string) = 
        let validCm(height:int) =
            height >= 150 && height <= 193
        let validIn(height:int) =
            height >= 59 && height <= 76 

        match height with
        | inch when height.Contains("in") -> 
            let value = inch.Substring(0, inch.IndexOf("in"))
            let couldParse, height = Int32.TryParse value
            if couldParse then validIn(height) else couldParse
        | centimeter when height.Contains("cm") -> 
            let value = centimeter.Substring(0, centimeter.IndexOf("cm"))
            let couldParse, height = Int32.TryParse value
            if couldParse then validCm(height) else couldParse
        | _ -> false

    let validHairColor(hairColor:string) =
       Regex.IsMatch(hairColor, "^#[a-f0-9]{6}$")

    let validEyeColor(eyeColor:string) = 
        match eyeColor with
        | "amb" -> true
        | "blu" -> true
        | "brn" -> true
        | "gry" -> true
        | "grn" -> true
        | "hzl" -> true
        | "oth" -> true
        | _ -> false
    
    let validPassportId(passportId:string) =
       Regex.IsMatch(passportId, "^[0-9]{9}$")

    //if not (validBirthYear(passport.BirthYear) && validIssueYear(passport.IssueYear) && validExpirationYear(passport.ExpirationYear) && validHeight(passport.Height) && validHairColor(passport.HairColor) && validEyeColor(passport.EyeColor) && validPassportId(passport.PassportId)) then
    //    printf "%s\n" (printPassport(passport))
    
    validBirthYear(passport.BirthYear) && validIssueYear(passport.IssueYear) && validExpirationYear(passport.ExpirationYear) && validHeight(passport.Height) && validHairColor(passport.HairColor) && validEyeColor(passport.EyeColor) && validPassportId(passport.PassportId)


let rec getPassportGroups(input:string list) =
    match input with 
    | [] -> ""
    | x :: xs -> 
        match x with 
        | "" -> "; " + getPassportGroups(xs)
        | _ -> x + " " + getPassportGroups(xs)

[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines("input.txt")

    let passportGroups = input |> Seq.toList
                               |> getPassportGroups
    
    let passports = passportGroups.Split(" ; ") |> Seq.toList
                                                |> List.where(fun x -> passportContainsAllNeccessaryFields(x))
                                                |> List.map(fun x -> passportMapper(x))
                                                //|> List.map(fun x -> printf "%s" (printPassport(x)))
                                                |> List.where(fun x -> passportFieldsAreValid(x))
                
    printf "%d" passports.Length

    0 // return an integer exit code
