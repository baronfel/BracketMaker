#light
namespace BracketMaker
module Team =

    type Team(name : string, division : string, players : seq<string>, weight : float) = 
        
        let name = name
        let division = division
        let players = players
        let weight = weight

        member this.Name with get() = name
        member this.Division with get() = division
        member this.Players with get() = players
        member this.Weight with get() = weight

        override this.ToString() =
            this.Name

    

