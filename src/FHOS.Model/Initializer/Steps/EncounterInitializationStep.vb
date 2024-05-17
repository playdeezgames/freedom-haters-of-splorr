Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class EncounterInitializationStep
    Inherits InitializationStep

    Private ReadOnly map As IMap

    Public Sub New(map As IMap)
        Me.map = map
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        For Each descriptor In ActorTypes.Descriptors.Where(Function(x) x.Value.SpawnCounts.ContainsKey(map.MapType))
            For Each dummy In Enumerable.Range(0, descriptor.Value.SpawnCounts(map.MapType))
                SpawnActor(descriptor.Value)
            Next
        Next
    End Sub

    Private Sub SpawnActor(descriptor As ActorTypeDescriptor)
        Dim locations = map.Locations.Where(Function(x) descriptor.CanSpawn(x) AndAlso x.Actor Is Nothing)
        If locations.Any Then
            descriptor.CreateActor(RNG.FromEnumerable(locations))
        End If
    End Sub
End Class
