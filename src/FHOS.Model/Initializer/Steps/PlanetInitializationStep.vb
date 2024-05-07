Imports FHOS.Persistence

Friend Class PlanetInitializationStep
    Inherits InitializationStep
    Private Const PlanetOrbitColumns = 11
    Private Const PlanetOrbitRows = 11
    Private ReadOnly location As Persistence.ILocation
    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim planet = location.Place
        planet.Map = planet.Universe.CreateMap(
            MapTypes.System,
            $"{planet.Name} Orbit",
            PlanetOrbitColumns,
            PlanetOrbitRows,
            LocationTypes.Void)
        PlaceBoundaries(planet, location)
        planet.Map.Place = planet
    End Sub
    Private Sub PlaceBoundaries(planet As IPlace, location As ILocation)
        Dim teleporter = location.CreateTeleporterTo()
        Dim identifier = planet.Identifier
        For Each corner In GetCorners(PlanetOrbitColumns, PlanetOrbitRows)
            PlaceBoundary(planet.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(PlanetOrbitColumns, PlanetOrbitRows)
            PlaceBoundary(planet.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planet.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
