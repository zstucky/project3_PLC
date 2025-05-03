open Giraffe
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http

open Parse
open Fun


type ParseRequest  = { expr: string } // will be received from front end
type ParseResponse = { bracketExpr: string } // will be sent back to the front end

[<EntryPoint>]
let main (args: string[]) =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()
    app.UseStaticFiles() |> ignore

    let parseHandler : HttpHandler =
        fun next ctx ->
            task {
                // get the JSON 
                let! req = ctx.BindJsonAsync<ParseRequest>()
                // parse into AST using parser
                let ast     = Parse.fromString req.expr
                // convert from AST to bracket notation
                let bracket = Fun.print ast
                // return json 
                return! json { bracketExpr = bracket } next ctx
            }

    let webApp =
        choose [
            POST >=> route "/api/parse" >=> parseHandler
            route "/" >=> htmlFile "./wwwroot/index.html"
        ]
    app.UseGiraffe(webApp)

    // temp testing
    printfn "%A" Parse.e1
    printfn "%A" Parse.e2
    eval Parse.e1 [] |> printfn "%A"    
    eval Parse.e2 [] |> printfn "%A"
    print Parse.e1 |> printfn "%A"
    print Parse.e2 |> printfn "%A"

    app.Run()

    0 // Exit code

