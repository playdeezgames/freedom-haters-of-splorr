Imports FHOS.Persistence

Friend Class SatelliteInitializationStep
    Inherits InitializationStep
    Private Const SatelliteOrbitColumns = 9
    Private Const SatelliteOrbitRows = 9
    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim satellite = location.Place
        satellite.Map = satellite.Universe.CreateMap(
            MapTypes.System,
            $"{satellite.Name} Orbit",
            SatelliteOrbitColumns,
            SatelliteOrbitRows,
            LocationTypes.Void)
        PlaceBoundaries(satellite, location)
        satellite.Map.Place = satellite
    End Sub
    Private Sub PlaceBoundaries(planet As IPlace, location As ILocation)
        Dim teleporter = location.CreateTeleporterTo()
        Dim identifier = planet.Identifier
        For Each corner In GetCorners(SatelliteOrbitColumns, SatelliteOrbitRows)
            PlaceBoundary(planet.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(SatelliteOrbitColumns, SatelliteOrbitRows)
            PlaceBoundary(planet.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planet.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
