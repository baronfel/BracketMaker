#light

// JARGON
// Pod - A container for a specific type of data in a Wolfram Alpha query response.  Pods have titles and describe their content, though we only want a certain type of pod.

open System
open System.Net
open System.Xml
open System.IO
open System.Xml.Linq

let getDoc (x : StreamReader) =
    XDocument.Load x

let xName value = XName.Get value

let getPlayerList (players : seq<XElement>) =
    let playerNames = players |> Seq.map ( fun x -> x.Attribute(xName "name").Value )
    playerNames

let getTeamLists =
    let teamXDoc = XDocument.Load "..\..\TeamRosters.xml"
    let teamNodes = teamXDoc.Elements(xName "team")
    let teamMaps = seq { for node in teamNodes -> (node.Attribute(xName "name").Value, getPlayerList (node.Elements(xName "player"))) }
    teamMaps

// Helper method to read the Birth rate info in the target Pod
let parsePodValue (pod : XElement) = 
    let start = pod.Value.IndexOf("(")+1
    let parenEnd = pod.Value.IndexOf(")")
    Convert.ToDouble(pod.Value.Substring(start,  parenEnd-start-1))

// Parses the WA xml to get the frequency value
let getNameValue (xDoc : XDocument) =
    let query = xDoc.Element(xName "queryresult")
    let pods = query.Elements( xName "pod")
    let targetPod = pods |> Seq.tryFind(fun x -> (x.Attribute(xName "title").Value.Contains("births") || x.Attribute(xName "title").Value.Contains("Basic information for the United States")))
    
    match targetPod with 
    | Some(targetPod) -> parsePodValue targetPod
    | None -> 0.0

// For a given name, queries Wolfram Alpha for the name frequencies for that name in the US
let getNameFrequency (name  : String) =
    let baseUrl = "http://api.wolframalpha.com/v2/query?input="
    let apiKeySection = "&appid=QYJPEA-G6QWXLRGJA"
    let finalUrl = String.Format("{0}{1}{2}", baseUrl, name, apiKeySection).Replace(" ", "%20")
    let response = WebRequest.Create(finalUrl).GetResponse() 
    let reader = new StreamReader(response.GetResponseStream())
  
    getDoc reader 
    |> getNameValue

// Averages the frequencies of the players in a team.
let getTeamWeight (players : seq<string>) =
    let weight = players |> Seq.map (fun x -> getNameFrequency x) |> Seq.average
    weight

// Entry point - reads in the xml file and requests weights.
let getTeamWeightsAsync =
    let startAsync = DateTime.Now
    let teamList = getTeamLists
    let teamWeights = Async.Parallel [ for (teamName, players) in teamList -> async {return teamName, getTeamWeight players }] |> Async.RunSynchronously
    let endAsync = DateTime.Now
    Console.WriteLine("Aync took {0}", endAsync.Subtract(startAsync))
    teamWeights

let getTeamWeights =
    let startSync = DateTime.Now
    let teamList = getTeamLists
    let teamWeights = seq { for (teamName, players) in teamList -> teamName, getTeamWeight players }
    let endSync = DateTime.Now
    Console.WriteLine("Sync took {0}", endSync.Subtract(startSync))
    teamWeights


//let weights = getTeamWeights
let weightsAsync = getTeamWeightsAsync
//printfn "%A" weights
printfn "%A" weightsAsync
Console.ReadLine() |> ignore
