Imports FHOS.Data

Friend Class Location
    Inherits LocationDataClient
    Implements ILocation

    Protected Sub New(universeData As Data.UniverseData, locationId As Integer)
        MyBase.New(universeData, locationId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, locationId As Integer?) As ILocation
        If locationId.HasValue Then
            Return New Location(universeData, locationId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Map As IMap Implements ILocation.Map
        Get
            Return Persistence.Map.FromId(UniverseData, EntityData.Statistics(StatisticTypes.MapId))
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
                Return Persistence.Actor.FromId(UniverseData, actorId)
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

    Public Property Place As IPlace Implements ILocation.Place
        Get
            Dim starSystemId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.PlaceId, starSystemId) Then
                Return Persistence.Place.FromId(UniverseData, starSystemId)
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

    Public Property TargetLocation As ILocation Implements ILocation.TargetLocation
        Get
            Return Location.FromId(UniverseData, GetStatistic(StatisticTypes.TargetLocationId))
        End Get
        Set(value As ILocation)
            SetStatistic(StatisticTypes.TargetLocationId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property HasTargetLocation As Boolean Implements ILocation.HasTargetLocation
        Get
            Return EntityData.Statistics.ContainsKey(StatisticTypes.TargetLocationId)
        End Get
    End Property

    Public Property IsEdge As Boolean Implements ILocation.IsEdge
        Get
            Return Flags(FlagTypes.IsEdge)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsEdge) = value
        End Set
    End Property

    Public Function CreateActor(actorType As String, name As String) As IActor Implements ILocation.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.LocationId, Id},
                                        {StatisticTypes.Facing, 0}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.ActorType, actorType},
                                        {MetadataTypes.Name, name}
                                    }
                                 }
        Dim actorId As Integer = UniverseData.Actors.CreateOrRecycle(actorData)
        Dim actor = Persistence.Actor.FromId(UniverseData, actorId)
        Me.Actor = actor
        Return actor
    End Function
End Class
