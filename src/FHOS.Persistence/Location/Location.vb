﻿Friend Class Location
    Inherits LocationDataClient
    Implements ILocation

    Public Sub New(universeData As Data.UniverseData, locationId As Integer)
        MyBase.New(universeData, locationId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ILocation.Id
        Get
            Return LocationId
        End Get
    End Property

    Public ReadOnly Property Map As IMap Implements ILocation.Map
        Get
            Return New Map(UniverseData, LocationData.Statistics(StatisticTypes.MapId))
        End Get
    End Property

    Public ReadOnly Property Column As Integer Implements ILocation.Column
        Get
            Return LocationData.Statistics(StatisticTypes.Column)
        End Get
    End Property

    Public ReadOnly Property Row As Integer Implements ILocation.Row
        Get
            Return LocationData.Statistics(StatisticTypes.Row)
        End Get
    End Property

    Public Property LocationType As String Implements ILocation.LocationType
        Get
            Return LocationData.Metadatas(MetadataTypes.LocationType)
        End Get
        Set(value As String)
            LocationData.Metadatas(MetadataTypes.LocationType) = value
        End Set
    End Property

    Public Property Actor As IActor Implements ILocation.Actor
        Get
            Dim actorId As Integer
            If LocationData.Statistics.TryGetValue(StatisticTypes.ActorId, actorId) Then
                Return New Actor(UniverseData, actorId)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value Is Nothing Then
                LocationData.Statistics.Remove(StatisticTypes.ActorId)
                Return
            End If
            LocationData.Statistics(StatisticTypes.ActorId) = value.Id
        End Set
    End Property

    Public Property StarSystem As IStarSystem Implements ILocation.StarSystem
        Get
            Dim starSystemId As Integer
            If LocationData.Statistics.TryGetValue(StatisticTypes.StarSystemId, starSystemId) Then
                Return New StarSystem(UniverseData, starSystemId)
            End If
            Return Nothing
        End Get
        Set(value As IStarSystem)
            If value IsNot Nothing Then
                LocationData.Statistics(StatisticTypes.StarSystemId) = value.Id
            Else
                LocationData.Statistics.Remove(StatisticTypes.StarSystemId)
            End If
        End Set
    End Property

    Public Property Teleporter As ITeleporter Implements ILocation.Teleporter
        Get
            Dim teleporterId As Integer
            If LocationData.Statistics.TryGetValue(StatisticTypes.TeleporterId, teleporterId) Then
                Return New Teleporter(UniverseData, teleporterId)
            End If
            Return Nothing
        End Get
        Set(value As ITeleporter)
            If value IsNot Nothing Then
                LocationData.Statistics(StatisticTypes.TeleporterId) = value.Id
            Else
                LocationData.Statistics.Remove(StatisticTypes.TeleporterId)
            End If
        End Set
    End Property

    Public Property Tutorial As String Implements ILocation.Tutorial
        Get
            Dim result As String = Nothing
            If LocationData.Metadatas.TryGetValue(MetadataTypes.Tutorial, result) Then
                Return result
            End If
            Return Nothing
        End Get
        Set(value As String)
            If value Is Nothing Then
                LocationData.Metadatas.Remove(MetadataTypes.Tutorial)
            Else
                LocationData.Metadatas(MetadataTypes.Tutorial) = value
            End If
        End Set
    End Property

    Public ReadOnly Property HasTeleporter As Boolean Implements ILocation.HasTeleporter
        Get
            Return LocationData.Statistics.ContainsKey(StatisticTypes.TeleporterId)
        End Get
    End Property

    Public Property Star As IStar Implements ILocation.Star
        Get
            Dim starId As Integer
            If LocationData.Statistics.TryGetValue(StatisticTypes.StarId, starId) Then
                Return New Star(UniverseData, starId)
            End If
            Return Nothing
        End Get
        Set(value As IStar)
            If value Is Nothing Then
                LocationData.Statistics.Remove(StatisticTypes.StarId)
            Else
                LocationData.Statistics(StatisticTypes.StarId) = value.Id
            End If
        End Set
    End Property

    Public Sub SetFlag(flag As String) Implements ILocation.SetFlag
        LocationData.Flags.Add(flag)
    End Sub

    Public Function HasFlag(name As String) As Boolean Implements ILocation.HasFlag
        Return LocationData.Flags.Contains(name)
    End Function
End Class