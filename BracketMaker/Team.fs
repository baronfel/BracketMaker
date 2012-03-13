#light
namespace BracketMaker
module Team =

    type Team(name : string, division : string, players : seq<string>) = 
        
        let name = name
        let division = division
        let players = players
        let weight = NameWeight.getTeamWeight players


        member this.Name with get() = name
        member this.Division with get() = division
        member this.Players with get() = players
        member this.Weight with get() = NameWeight.getTeamWeight players

        override this.ToString() =
            this.Name

    

