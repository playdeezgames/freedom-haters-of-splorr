Imports FHOS.Persistence

Friend Class SatelliteOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim satellite = location.Place
        Dim map = MapTypes.Descriptors(MapTypes.SatelliteOrbit).CreateMap($"{satellite.Properties.Name} Orbit", satellite.Universe)
        satellite.Properties.Map = map
        location.Actor.Properties.Interior = map
        PlaceBoundaryActors(satellite, location.Actor, satellite.Properties.Map.Size.Columns, satellite.Properties.Map.Size.Rows)
        PlaceSatellite(satellite)
        addStep(New EncounterInitializationStep(satellite.Properties.Map), True)
    End Sub

    Private Sub PlaceSatellite(satellite As IPlace)
        Dim planetCenterColumn = satellite.Properties.Map.Size.Columns \ 2
        Dim planetCenterRow = satellite.Properties.Map.Size.Rows \ 2
        For Each delta In Grid3x3.Descriptors
            PlaceSatelliteSection(satellite.Subtype, satellite.Properties.Map.GetLocation(planetCenterColumn + delta.Value.Delta.X, planetCenterRow + delta.Value.Delta.Y), delta.Key)
        Next
    End Sub

    Private Sub PlaceSatelliteSection(satelliteType As String, location As ILocation, sectionName As String)
        Dim locationType = SatelliteTypes.Descriptors(satelliteType).SectionLocationType(sectionName)
        With location
            .LocationType = locationType
        End With
    End Sub
End Class
