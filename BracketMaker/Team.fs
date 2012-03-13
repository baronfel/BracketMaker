#light
namespace BracketMaker
module Team =

    type Team(name : string, division : string, players : seq<string>) = 
        
        let name = name
        let division = division
        let players = players


        member this.Name with get() = name
        member this.Division with get() = division
        member this.Players with get() = players

        override this.ToString() =
            this.Name

        member this.getWeight() =
            NameWeight.getTeamWeight players

    

