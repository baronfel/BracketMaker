#light

namespace BracketMaker

open NameWeight
open System
open Team
open Match

module Program =

    BracketMaker.RosterGenerator.getTeamLists
    |> Seq.iter(fun x -> printfn "%s -- %e" x.Name x.Weight)
    Console.ReadKey() |> ignore