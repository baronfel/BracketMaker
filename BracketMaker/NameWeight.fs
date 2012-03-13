#light

// JARGON
// Pod - A container for a specific type of data in a Wolfram Alpha query response.  Pods have titles and describe their content, though we only want a certain type of pod.

namespace BracketMaker

open System
open System.Net
open System.Xml
open System.IO
open System.Xml.Linq

module NameWeight = 

    let getDoc (x : StreamReader) =
        XDocument.Load x

    let pmap f l =
        seq { for a in l -> async { return f a} } 
        |> Async.Parallel 
        |> Async.RunSynchronously

    let xName value = XName.Get value

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
        printfn "Getting name weight for player %s" name
        let baseUrl = "http://api.wolframalpha.com/v2/query?input="
        let apiKeySection = "&appid=QYJPEA-G6QWXLRGJA"
        let finalUrl = String.Format("{0}{1}{2}", baseUrl, name, apiKeySection).Replace(" ", "%20")
        let request = WebRequest.Create(finalUrl)
        let response = request.GetResponse() 
        let reader = new StreamReader(response.GetResponseStream())
  
        getDoc reader 
        |> getNameValue

    // Averages the frequencies of the players in a team.
    let getTeamWeight (players : seq<string>) =
        let weight = Async.Parallel [ for player in players -> async { return getNameFrequency player}] |> Async.RunSynchronously |> Seq.average
        weight