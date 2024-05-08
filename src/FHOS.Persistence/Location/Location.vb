Imports FHOS.Data

Friend Class Location
    Inherits LocationDataClient
    Implements ILocation

    Public Sub New(universeData As Data.UniverseData, locationId As Integer)
        MyBase.New(universeData, locationId)
    End Sub

    Public ReadOnly Property Map As IMap Implements ILocation.Map
        Get
            Return New Map(UniverseData, EntityData.Statistics(StatisticTypes.MapId))
        End Get
    End Property

    Public ReadOnly Property Column As Integer Implements ILocation.Column
        Get
            Return EntityData.Statistics(StatisticTypes.Column)
        End Get
    End Property

    Public ReadOnly Property Row As Integer Implements ILocation.Row
        Get
            Return EntityData.Statistics(StatisticTypes.Row)
        End Get
    End Property

    Public Property LocationType As String Implements ILocation.LocationType
        Get
            Return EntityData.Metadatas(MetadataTypes.LocationType)
        End Get
        Set(value As String)
            EntityData.Metadatas(MetadataTypes.LocationType) = value
        End Set
    End Property

    Public Property Actor As IActor Implements ILocation.Actor
        Get
            Dim actorId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.ActorId, actorId) Then
                Return New Actor(UniverseData, actorId)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value Is Nothing Then
                EntityData.Statistics.Remove(StatisticTypes.ActorId)
                Return
            End If
            EntityData.Statistics(StatisticTypes.ActorId) = value.Id
        End Set
    End Property

    Public Property Teleporter As ITeleporter Implements ILocation.Teleporter
        Get
            Dim teleporterId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.TeleporterId, teleporterId) Then
                Return New Teleporter(UniverseData, teleporterId)
            End If
            Return Nothing
        End Get
        Set(value As ITeleporter)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.TeleporterId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.TeleporterId)
            End If
        End Set
    End Property

    Public Property Tutorial As String Implements ILocation.Tutorial
        Get
            Dim result As String = Nothing
            If EntityData.Metadatas.TryGetValue(MetadataTypes.Tutorial, result) Then
                Return result
            End If
            Return Nothing
        End Get
        Set(value As String)
            If value Is Nothing Then
                EntityData.Metadatas.Remove(MetadataTypes.Tutorial)
            Else
                EntityData.Metadatas(MetadataTypes.Tutorial) = value
            End If
        End Set
    End Property

    Public ReadOnly Property HasTeleporter As Boolean Implements ILocation.HasTeleporter
        Get
            Return EntityData.Statistics.ContainsKey(StatisticTypes.TeleporterId)
        End Get
    End Property

    Public Property Place As IPlace Implements ILocation.Place
        Get
            Dim starSystemId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.PlaceId, starSystemId) Then
                Return New Place(UniverseData, starSystemId)
            End If
            Return Nothing
        End Get
        Set(value As IPlace)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.PlaceId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.PlaceId)
            End If
        End Set
    End Property

    Public Sub SetFlag(flag As String) Implements ILocation.SetFlag
        EntityData.Flags.Add(flag)
    End Sub

    Public Function HasFlag(name As String) As Boolean Implements ILocation.HasFlag
        Return EntityData.Flags.Contains(name)
    End Function

    Public Function CreateActor(actorType As String) As IActor Implements ILocation.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.LocationId, Id},
                                        {StatisticTypes.Facing, 1},
                                        {StatisticTypes.Turn, 1},
                                        {StatisticTypes.Jools, 0}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.ActorType, actorType}
                                    }
                                 }
        Dim actorId As Integer = UniverseData.Actors.CreateOrRecycle(actorData)
        Dim actor = New Actor(UniverseData, actorId)
        Me.Actor = actor
        Return actor
    End Function

    Public Function CreateTeleporterTo() As ITeleporter Implements ILocation.CreateTeleporterTo
        Dim teleporterData As New TeleporterData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.LocationId, Id}
                }
            }
        Dim teleporterId As Integer = UniverseData.Teleporters.CreateOrRecycle(teleporterData)
        Return New Teleporter(UniverseData, teleporterId)
    End Function
End Class
