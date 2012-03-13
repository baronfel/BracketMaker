#light

namespace BracketMaker

open Team

module Match =

    type Match (a : Match, b : Match) = 
        let left = a
        let right = b

        member this.Left with get() = left
        member this.Right with get() = right
        
        member this.winner() : Team =
            let wLeft = this.Left.winner().Weight
            let wRight = this.Right.winner().Weight

            if(wLeft > wRight) then this.Left.winner()
            else this.Right.winner()
            
        override this.ToString() = 
            this.Left.ToString() + " <--> " + this.Right.ToString()

