#light

namespace BracketMaker 

open System.Xml.Linq
open System.Net
open System.Xml
open System

module RosterGenerator =

    let xName value = XName.Get value

    let getPlayerList (players : seq<XElement>) =
        let playerNames = players |> Seq.map ( fun x -> x.Attribute(xName "name").Value )
        playerNames

    let getTeamLists =
        let teamXDoc = XDocument.Load "..\..\TeamRosters.xml"
        let teamNodes = teamXDoc.Elements(xName "team")
        let teamMaps = seq { for node in teamNodes -> (node.Attribute(xName "name").Value, getPlayerList (node.Elements(xName "player"))) }
        teamMaps

    let parseTeamId (href : string) =
        Convert.ToInt32(href.Substring(href.IndexOf("=")))

    let getPlayersOnTeam (id : int) =
        let rosterRequest = WebRequest.Create(String.Format( "http://stats.ncaa.org/team/index/10740?org_id={0}", id))
        let response = rosterRequest.GetResponse()
        let teamXDoc = XDocument.Load( response.GetResponseStream())

        // This is terrible, and I know it.  I can't seem to filter out string that contain 'stats_player_seq' but not 'stats_player_seq=-100', which is my desired set.
        let playerHrefs = teamXDoc.Descendants( xName "a") |> Seq.filter( fun a -> a.Attribute(xName "href").Value.Contains("stats_player_seq") && a.Attribute(xName "href").Value.Length.Equals 68)
        playerHrefs |> Seq.map (fun href -> href.Value.Split(','))
        |> Seq.map (fun arr -> Array.rev arr)
        |> Seq.map ( fun arr -> arr.[0] + arr.[1])

    
    let parseTeamRosters (idNameMap : seq<int * string>) =
        Async.Parallel [ for (id, name) in idNameMap -> async { return (name, getPlayersOnTeam id) }] |> Async.RunSynchronously

//    let getTeamListsFromNCAA =
//        let teamRequest = WebRequest.Create "http://stats.ncaa.org/team/inst_team_list#"
//        let response = teamRequest.GetResponse()
//        let teamXDoc = XDocument.Load(response.GetResponseStream())
//        // The team web page has a list of hyperlinks (<a/>) that have onclicks
//        let teamLinks = teamXDoc.Descendants(xName "a") 
//        let teamIdAndName = seq { for link in teamLinks -> (parseTeamId( link.Attribute(xName "href").Value), link.Value) }
//        parseTeamRosters teamIdAndName

