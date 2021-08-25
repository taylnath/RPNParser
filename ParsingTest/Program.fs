open System
open FParsec

type Expr = Num of float

let expr, exprRef = createParserForwardedToRef<Expr, unit>()
let pAtom : Parser<Expr, unit> -> Parser<Expr, unit> = between (spaces >>? pstring "(") (spaces >>? pstring ")")
let NumOp op = fun (Num a) (Num b) -> Num (op a b)
let pOperationBuilder opString op = pipe2 (spaces >>? pstring opString >>? spaces1 >>. expr) (spaces >>? expr) (NumOp op)
let pPlus = pOperationBuilder "+" (+)
let pTimes = pOperationBuilder "*" (*)
let pDiv = pOperationBuilder "/" (/)
let pSub = pOperationBuilder "-" (-)
let pNum = pfloat |>> Num
let pAtomExpr = spaces >>? pAtom expr .>>? spaces
do exprRef := choice [pTimes; pPlus; pDiv; pSub; pNum; pAtomExpr; ]
let pExpr = spaces >>? expr .>>? spaces .>> eof

let rec loop() =
    Console.WriteLine "Enter in an expression in reverse polish notation, i.e. \"+ 3 4\", \"(* 3 (+ 4 5))\" etc."
    Console.ReadLine()
    |> run pExpr
    |> (fun x ->
        match x with
        | Success (Num f, _, _) -> Console.WriteLine f
        | Failure (e, _, _) -> Console.WriteLine e
    )
    loop()
    

[<EntryPoint>]
let main argv =
    loop()
    0 // return an integer exit code