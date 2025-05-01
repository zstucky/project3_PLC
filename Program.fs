open Giraffe
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

open Parse
open Fun

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()
    app.UseStaticFiles() |> ignore


    // app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore
    let webApp =
        choose [
            route "/" >=> htmlFile "./wwwroot/index.html"
            // your other routes
        ]
    app.UseGiraffe(webApp)

    printfn "%A" Parse.e1

    printfn "%A" Parse.ex1
    // printfn "%A" Parse.ex2


    app.Run()

    0 // Exit code

