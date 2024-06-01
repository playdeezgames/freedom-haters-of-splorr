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
            Return Persistence.Map.FromId(UniverseData, EntityData.Statistics(LegacyStatisticTypes.MapId))
        End Get
    End Property

    Public ReadOnly Property Column As Integer Implements ILocation.Column
        Get
            Return EntityData.Statistics(LegacyStatisticTypes.Column)
        End Get
    End Property

    Public ReadOnly Property Row As Integer Implements ILocation.Row
        Get
            Return EntityData.Statistics(LegacyStatisticTypes.Row)
        End Get
    End Property

    Public Property EntityType As String Implements ILocation.EntityType
        Get
            Return EntityData.Metadatas(LegacyMetadataTypes.LocationType)
        End Get
        Set(value As String)
            EntityData.Metadatas(LegacyMetadataTypes.LocationType) = value
        End Set
    End Property

    Public Property Actor As IActor Implements ILocation.Actor
        Get
            Dim actorId As Integer
            If EntityData.Statistics.TryGetValue(LegacyStatisticTypes.ActorId, actorId) Then
                Return Persistence.Actor.FromId(UniverseData, actorId)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value Is Nothing Then
                EntityData.Statistics.Remove(LegacyStatisticTypes.ActorId)
                Return
            End If
            EntityData.Statistics(LegacyStatisticTypes.ActorId) = value.Id
        End Set
    End Property

    Public Property Tutorial As String Implements ILocation.Tutorial
        Get
            Dim result As String = Nothing
            If EntityData.Metadatas.TryGetValue(LegacyMetadataTypes.Tutorial, result) Then
                Return result
            End If
            Return Nothing
        End Get
        Set(value As String)
            If value Is Nothing Then
                EntityData.Metadatas.Remove(LegacyMetadataTypes.Tutorial)
            Else
                EntityData.Metadatas(LegacyMetadataTypes.Tutorial) = value
            End If
        End Set
    End Property

    Public Property TargetLocation As ILocation Implements ILocation.TargetLocation
        Get
            Return Location.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.TargetLocationId))
        End Get
        Set(value As ILocation)
            SetStatistic(LegacyStatisticTypes.TargetLocationId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property HasTargetLocation As Boolean Implements ILocation.HasTargetLocation
        Get
            Return EntityData.Statistics.ContainsKey(LegacyStatisticTypes.TargetLocationId)
        End Get
    End Property

    Public Property IsEdge As Boolean Implements ILocation.IsEdge
        Get
            Return Flags(LegacyFlagTypes.IsEdge)
        End Get
        Set(value As Boolean)
            Flags(LegacyFlagTypes.IsEdge) = value
        End Set
    End Property

    Public Function CreateActor(actorType As String, name As String) As IActor Implements ILocation.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {LegacyStatisticTypes.LocationId, Id},
                                        {LegacyStatisticTypes.Facing, 0}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {LegacyMetadataTypes.ActorType, actorType},
                                        {LegacyMetadataTypes.Name, name}
                                    }
                                 }
        Dim actorId As Integer = UniverseData.Actors.CreateOrRecycle(actorData)
        Dim actor = Persistence.Actor.FromId(UniverseData, actorId)
        Me.Actor = actor
        Return actor
    End Function
End Class
