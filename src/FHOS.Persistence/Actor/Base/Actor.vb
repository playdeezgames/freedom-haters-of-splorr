Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActor
        If actorId.HasValue Then
            Return New Actor(universeData, actorId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Family As IActorFamily Implements IActor.Family
        Get
            Return ActorFamily.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Properties As IActorProperties Implements IActor.Properties
        Get
            Return ActorProperties.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Equipment As IActorEquipment Implements IActor.Equipment
        Get
            Return ActorEquipment.FromId(UniverseData, Id)
        End Get
    End Property

    Public Property YokedActor(yokeType As String) As IActor Implements IActor.YokedActor
        Get
            Dim id As Integer
            If EntityData.YokedActors.TryGetValue(yokeType, id) Then
                Return Actor.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value IsNot Nothing Then
                EntityData.YokedActors(yokeType) = value.Id
            Else
                EntityData.YokedActors.Remove(yokeType)
            End If
        End Set
    End Property

    Public Property YokedStore(yokeType As String) As IStore Implements IActor.YokedStore
        Get
            Dim id As Integer
            If EntityData.YokedStores.TryGetValue(yokeType, id) Then
                Return Store.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IStore)
            If value IsNot Nothing Then
                EntityData.YokedStores(yokeType) = value.Id
            Else
                EntityData.YokedStores.Remove(yokeType)
            End If
        End Set
    End Property

    Public Property Location As ILocation Implements IActor.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.Statistics(PersistenceStatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(PersistenceStatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(PersistenceStatisticTypes.LocationId) = value.Id
                value.Actor = Actor.FromId(UniverseData, Id)
            End If
        End Set
    End Property

    Public Property YokedGroup(yokeType As String) As IGroup Implements IActor.YokedGroup
        Get
            Dim id As Integer
            If EntityData.YokedGroups.TryGetValue(yokeType, id) Then
                Return Group.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IGroup)
            If value IsNot Nothing Then
                EntityData.YokedGroups(yokeType) = value.Id
            Else
                EntityData.YokedGroups.Remove(yokeType)
            End If
        End Set
    End Property
End Class
