// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Salesman

[<EntryPoint>]
let main argv = 
    let map = new Map("Uk-Postcodes-Towns.csv")
    let loc = map.["Aberdeen"]
    printfn "Localtion data - %s" (loc.ToString())
    0 // return an integer exit code