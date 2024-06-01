Imports FHOS.Persistence

Friend Class SatelliteOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim satellite = location.Actor
        Dim map = MapTypes.Descriptors(MapTypes.SatelliteOrbit).CreateMap($"{satellite.Properties.Name} Orbit", satellite.Universe)
        location.Actor.Properties.Interior = map
        PlaceBoundaryActors(satellite, map.Size.Columns, map.Size.Rows)
        PlaceSatellite(satellite)
        addStep(New EncounterInitializationStep(map), True)
    End Sub

    Private Sub PlaceSatellite(satellite As IActor)
        Dim centerColumn = satellite.Properties.Interior.Size.Columns \ 2
        Dim centerRow = satellite.Properties.Interior.Size.Rows \ 2
        For Each delta In Grid3x3.Descriptors
            PlaceSatelliteSection(satellite.Descriptor.Subtype, satellite.Properties.Interior.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y), delta.Key, satellite.Properties.Groups(GroupTypes.Satellite))
        Next
    End Sub

    Private Sub PlaceSatelliteSection(satelliteType As String, location As ILocation, sectionName As String, satelliteGroup As IGroup)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeSatelliteSection(satelliteType, sectionName)).CreateActor(location, satelliteGroup.EntityName)
        location.LocationType = LocationTypes.Satellite
        actor.Properties.Groups(GroupTypes.Satellite) = satelliteGroup
    End Sub
End Class
