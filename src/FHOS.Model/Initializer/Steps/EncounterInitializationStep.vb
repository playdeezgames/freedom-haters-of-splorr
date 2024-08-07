﻿Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class EncounterInitializationStep
    Inherits InitializationStep

    Private ReadOnly map As IMap

    Public Sub New(map As IMap)
        Me.map = map
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return $"Initializing encounters for map {map.EntityName}..."
        End Get
    End Property

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        For Each ActorDescriptor In ActorTypes.Descriptors.Where(Function(x) x.Value.SpawnRolls.ContainsKey(map.EntityType))
            Dim spawnCount = RNG.RollDice(ActorDescriptor.Value.SpawnRolls(map.EntityType))
            For Each dummy In Enumerable.Range(0, spawnCount)
                SpawnActor(ActorDescriptor.Value)
            Next
        Next
    End Sub

    Private Sub SpawnActor(descriptor As ActorTypeDescriptor)
        Dim locations = map.Locations.Where(Function(x) descriptor.CanSpawn(x) AndAlso x.Actor Is Nothing)
        If locations.Any Then
            descriptor.CreateActor(RNG.FromEnumerable(locations), descriptor.ActorType)
        End If
    End Sub
End Class
