#light

namespace BracketMaker

open NameWeight
open System

module BracketMaker =

    let weights = NameWeight.getTeamWeightsAsync BracketMaker.RosterGenerator.getTeamLists
    printfn "%A" weights
    Console.ReadKey() |> ignore


