#light

namespace BracketMaker

open NameWeight
open System

module Program =

    let weights = Async.Parallel [ for team in BracketMaker.RosterGenerator.getTeamLists -> async { return team, team.Weight } ] |> Async.RunSynchronously |> Array.toSeq
    weights |> Seq.iter( fun (a, b) -> printfn "%s - %e" a.Name b)
    Console.ReadKey() |> ignore