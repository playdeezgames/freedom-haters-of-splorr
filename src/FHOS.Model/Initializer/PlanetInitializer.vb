Imports FHOS.Persistence

Friend Module PlanetInitializer
    Private Const PlanetVicinityColumns = 15
    Private Const PlanetVicinityRows = 15
    Friend Sub Initialize(planet As IPlanet, planetLocation As ILocation)
        planet.Map = planet.Universe.CreateMap(
            MapTypes.System,
            $"{planet.Name} Vicinity",
            PlanetVicinityColumns,
            PlanetVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(planet, planetLocation)
    End Sub
    Private Sub PlaceBoundaries(planet As IPlanet, planetLocation As ILocation)
        Dim teleporter = planet.Universe.CreateTeleporter(planetLocation)
        Dim starFlag = planet.Name
        For Each corner In GetCorners(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planet.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planet.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planet.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Module
