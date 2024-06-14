Imports FHOS.Persistence

Friend Class SatelliteOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim satellite = location.Actor
        Dim map = MapTypes.Descriptors(MapTypes.SatelliteOrbit).CreateMap($"{satellite.EntityName} Orbit", satellite.Universe)
        location.Actor.Interior = map
        PlaceBoundaryActors(satellite, map.Size.Columns, map.Size.Rows)
        PlaceSatellite(satellite)
        addStep(New EncounterInitializationStep(map), True)
    End Sub

    Private Sub PlaceSatellite(satellite As IActor)
        Dim centerColumn = satellite.Interior.Size.Columns \ 2
        Dim centerRow = satellite.Interior.Size.Rows \ 2
        For Each delta In Grid3x3.Descriptors
            PlaceSatelliteSection(satellite.Descriptor.Subtype, satellite.Interior.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y), delta.Key, satellite.Yokes.Group(YokeTypes.Satellite))
        Next
    End Sub

    Private Sub PlaceSatelliteSection(satelliteType As String, location As ILocation, sectionName As String, satelliteGroup As IGroup)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeSatelliteSection(satelliteType, sectionName)).CreateActor(location, satelliteGroup.EntityName)
        location.EntityType = LocationTypes.Satellite
        actor.Yokes.Group(YokeTypes.Satellite) = satelliteGroup
    End Sub
End Class
