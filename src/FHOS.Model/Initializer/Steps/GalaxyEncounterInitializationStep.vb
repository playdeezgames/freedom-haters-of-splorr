Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class GalaxyEncounterInitializationStep
    Inherits InitializationStep

    Private ReadOnly map As IMap
    Private ReadOnly embarkSettings As EmbarkSettings

    Public Sub New(map As Persistence.IMap, embarkSettings As EmbarkSettings)
        Me.map = map
        Me.embarkSettings = embarkSettings
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        For Each descriptor In ActorTypes.Descriptors
            For Each dummy In Enumerable.Range(0, descriptor.Value.SpawnCount)
                SpawnActor(descriptor.Value)
            Next
        Next
    End Sub

    Private Sub SpawnActor(descriptor As ActorTypeDescriptor)
        Dim locations = map.Locations.Where(Function(x) descriptor.CanSpawn(x))
        If locations.Any Then
            descriptor.CreateActor(RNG.FromEnumerable(locations))
        End If
    End Sub
End Class
