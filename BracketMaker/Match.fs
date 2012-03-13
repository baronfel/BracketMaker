#light

namespace BracketMaker

open Team

module Match =

    type Match (a : Team, b : Team) = 
        let left = a
        let right = b

        member this.Left with get() = left
        member this.Right with get() = right
        
        member this.winner() =
            let wLeft = this.Left.Weight
            let wRight = this.Right.Weight

            if(wLeft > wRight) then this.Left
            else this.Right
            
        override this.ToString() = 
            this.Left.ToString() + " <--> " + this.Right.ToString()

