#light

namespace BracketMaker

open NameWeight
open System
open Team
open Match

module Program =

    let compareWithOthers (x :Team, others : seq<Team>) =
        others |> Seq.iter(fun y -> Console.WriteLine("The winner between {0} and {1} is {2}", x.ToString(), y.ToString(), (new Match(x, y)).winner().ToString()))

    let teams = BracketMaker.RosterGenerator.getTeamLists
    teams
    |> Seq.iter(fun x->compareWithOthers(x, teams))

    
    Console.ReadKey() |> ignore