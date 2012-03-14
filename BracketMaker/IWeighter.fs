#light
namespace BracketMaker

open Player

module IWeighter =
    type IWeighter =
        abstract member getTeamWeight : seq<Player> -> float