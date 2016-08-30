module Salesman

open System
open System.Collections.Generic
open System.IO

type MapLocation(name : string, longitude : double, latitude : double) = 
    member this.Name = name
    member this.Longitude = longitude
    member this.Latitude = latitude
    override this.ToString() = sprintf "%s %A,%A" this.Name this.Latitude this.Longitude

type Map(filePath : string) = 
  
    let locations = Dictionary(HashIdentity.Structural)
    
    let createMapLocation (value : string) = 
        let items = value.Split [| ',' |]
        let name = items.[1].Replace("\"", "")
        let longitute = Double.Parse(items.[5])
        let latitude = Double.Parse(items.[4])
        new MapLocation(name, longitute, latitude)
    
    let loadMap filePath = 
        File.ReadAllLines(filePath)
        |> Seq.map (createMapLocation)
        |> Seq.distinctBy (fun x -> x.Name)
        |> Seq.iter (fun loc -> locations.Add(loc.Name, loc))
    
    do loadMap filePath
  
    member this.Item 
        with get (town) = locations.[town]

    member this.Locations = locations.Values |> Seq.sortBy (fun x -> x.Name)
  
    static member distance (tuple : MapLocation * MapLocation) = 
        let start, finish = tuple
        let latitudeToMiles = 69.1
        let longitudeToMiles = 53.0
        
        let x = (longitudeToMiles * (start.Longitude - finish.Longitude)) ** 2.0
        let y = (latitudeToMiles * (start.Latitude - finish.Latitude)) ** 2.0
        sqrt (x + y)

type Trip (start:MapLocation, finish:MapLocation, wayPoints : MapLocation[] ) =
    
    member this.TotalDistance =
        let mutable from = start
        let mutable tripTotal = 0.0

        for wayPoint in wayPoints do
            tripTotal <- tripTotal + Map.distance (from, wayPoint)
            from <- wayPoint
        
        tripTotal + Map.distance (from, finish)

    member this.Start = start
    member this.Finish = finish
    member this.WayPoints = wayPoints

    override this.ToString() =
        let wp = this.WayPoints |> Seq.map(fun c->c.Name) |> String.concat ","
        sprintf "Strting %s via %A Ending %s" this.Start.Name wp this.Finish.Name