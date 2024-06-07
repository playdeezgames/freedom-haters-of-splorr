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

    Public ReadOnly Property Position As (Column As Integer, Row As Integer) Implements ILocation.Position
        Get
            Return (EntityData.Statistics(LegacyStatisticTypes.Column), EntityData.Statistics(LegacyStatisticTypes.Row))
        End Get
    End Property

    Public Function CreateActor(actorType As String, name As String) As IActor Implements ILocation.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {LegacyStatisticTypes.LocationId, Id}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {LegacyMetadataTypes.EntityType, actorType},
                                        {LegacyMetadataTypes.Name, name}
                                    }
                                 }
        Dim actorId As Integer = UniverseData.Actors.CreateOrRecycle(actorData)
        Dim actor = Persistence.Actor.FromId(UniverseData, actorId)
        Me.Actor = actor
        Return actor
    End Function
End Class
