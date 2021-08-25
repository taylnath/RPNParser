// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open FParsec

type Expr = Num of float

let expr, exprRef = createParserForwardedToRef<Expr, unit>()

let pAtom : Parser<Expr, unit> -> Parser<Expr, unit> = between (pstring "(") (pstring ")")
let pOperationBuilder op = pipe2 (spaces |>> ignore >>. pstring op >>. spaces >>. pfloat) (spaces >>. pfloat) 
let pOperationBuilder2 op = pipe2 (spaces |>> ignore >>. pstring op >>. spaces >>. expr) (spaces >>. expr) 
let pPlus = pOperationBuilder2 "+" (fun (Num a) (Num b) -> Num (a + b))
let pTimes = pOperationBuilder2 "*" (fun (Num a) (Num b) -> Num (a * b))
let pNum = pfloat |>> Num

let pExpr = spaces >>. pAtom expr .>> spaces

do exprRef := choice [
   pTimes
   pPlus
   pNum
   pExpr
]

let rec loop() =
    Console.WriteLine "Enter in a + expression in rpn"
    Console.ReadLine()
    |> run pExpr
    |> (fun x ->
        match x with
        | Success (Num f, _, _) -> Console.WriteLine f
        | Success (e, _, _) -> Console.WriteLine e
        | Failure (e, _, _) -> Console.WriteLine e
    )
    loop()
    

[<EntryPoint>]
let main argv =
    loop()
    0 // return an integer exit code