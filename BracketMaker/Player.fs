#light

namespace BracketMaker

module Player =

    type Player(name : string, position : string, stats : seq<string * float>) =
        let name = name
        let position = position
        let stats = stats

        member this.Name with get() = name
        member this.Position with get() = position
        member this.Stats with get() = stats