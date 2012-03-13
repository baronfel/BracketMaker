#light

namespace BracketMaker 

open System.Xml.Linq
open System.Net
open System.Xml
open System
open Team

module RosterGenerator =

    let xName value = XName.Get value

    let getPlayerList (players: string) =
        players.Split '\n' |> Array.toSeq |> Seq.filter( fun x -> x.Length > 0)

    let getTeamLists =
        let teamXDoc = XDocument.Load "..\..\TeamRosters.xml"
        let teamNodes = teamXDoc.Descendants(xName "team")
        let teamMaps = seq { for node in teamNodes -> new Team(node.Attribute(xName "name").Value, node.Attribute(xName "div").Value, getPlayerList node.Value) }
        teamMaps

