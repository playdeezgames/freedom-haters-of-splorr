Imports FHOS.Data

Friend Class Location
    Inherits LocationDataClient
    Implements ILocation

    Protected Sub New(universeData As Data.IUniverseData, locationId As Integer)
        MyBase.New(universeData, locationId)
    End Sub

    Friend Shared Function FromId(universeData As IUniverseData, locationId As Integer?) As ILocation
        If locationId.HasValue Then
            Return New Location(universeData, locationId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Map As IMap Implements ILocation.Map
        Get
            Return Persistence.Map.FromId(UniverseData, EntityData.GetStatistic(PersistenceStatisticTypes.MapId))
        End Get
    End Property

    Public Property Actor As IActor Implements ILocation.Actor
        Get
            Return Persistence.Actor.FromId(UniverseData, EntityData.GetStatistic(PersistenceStatisticTypes.ActorId))
        End Get
        Set(value As IActor)
            EntityData.SetStatistic(PersistenceStatisticTypes.ActorId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property Position As (Column As Integer, Row As Integer) Implements ILocation.Position
        Get
            Return (EntityData.GetStatistic(PersistenceStatisticTypes.Column).Value, EntityData.GetStatistic(PersistenceStatisticTypes.Row).Value)
        End Get
    End Property

    Public Function CreateActor(actorType As String, name As String) As IActor Implements ILocation.CreateActor
        Dim actorId As Integer = UniverseData.NextActorId
        Dim actorData = New ActorData(actorId, statistics:=New Dictionary(Of String, Integer) From
                                    {
                                        {PersistenceStatisticTypes.LocationId, Id}
                                    }) With
                                 {
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {LegacyMetadataTypes.EntityType, actorType},
                                        {LegacyMetadataTypes.Name, name}
                                    }
                                 }
        UniverseData.Actors.Add(actorId, actorData)
        Dim actor = Persistence.Actor.FromId(UniverseData, actorId)
        Me.Actor = actor
        Return actor
    End Function
End Class
