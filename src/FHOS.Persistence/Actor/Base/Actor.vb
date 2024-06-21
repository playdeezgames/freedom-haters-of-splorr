Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Protected Sub New(universeData As Data.IUniverseData, connection As SqliteConnection, actorId As Integer)
        MyBase.New(universeData, connection, actorId)
    End Sub

    Friend Shared Function FromId(universeData As IUniverseData, connection As SqliteConnection, actorId As Integer?) As IActor
        If actorId.HasValue Then
            Return New Actor(universeData, connection, actorId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Family As IActorFamily Implements IActor.Family
        Get
            Return ActorFamily.FromId(UniverseData, Connection, Id)
        End Get
    End Property

    Public ReadOnly Property Equipment As IActorEquipment Implements IActor.Equipment
        Get
            Return ActorEquipment.FromId(UniverseData, Connection, Id)
        End Get
    End Property

    Public Property Location As ILocation Implements IActor.Location
        Get
            Return Persistence.Location.FromId(UniverseData, Connection, EntityData.GetStatistic(PersistenceStatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.GetStatistic(PersistenceStatisticTypes.LocationId).Value Then
                Location.Actor = Nothing
                EntityData.SetStatistic(PersistenceStatisticTypes.LocationId, value.Id)
                value.Actor = Actor.FromId(UniverseData, Connection, Id)
            End If
        End Set
    End Property

    Public Property Interior As IMap Implements IActor.Interior
        Get
            Return Map.FromId(UniverseData, Connection, GetStatistic(PersistenceStatisticTypes.MapId))
        End Get
        Set(value As IMap)
            SetStatistic(PersistenceStatisticTypes.MapId, value?.Id)
        End Set
    End Property

    Public Property Costume As String Implements IActor.Costume
        Get
            Return GetMetadata(LegacyMetadataTypes.Costume)
        End Get
        Set(value As String)
            SetMetadata(LegacyMetadataTypes.Costume, value)
        End Set
    End Property

    Public ReadOnly Property Yokes As IActorYokes Implements IActor.Yokes
        Get
            Return ActorYokes.FromId(UniverseData, Connection, Id)
        End Get
    End Property

    Public ReadOnly Property Inventory As IActorInventory Implements IActor.Inventory
        Get
            Return ActorInventory.FromId(UniverseData, Connection, Id)
        End Get
    End Property

    Public ReadOnly Property Offers As IActorOffers Implements IActor.Offers
        Get
            Return ActorOffers.FromId(UniverseData, Connection, Id)
        End Get
    End Property

    Public ReadOnly Property Prices As IActorPrices Implements IActor.Prices
        Get
            Return ActorPrices.FromId(UniverseData, Connection, Id)
        End Get
    End Property
End Class
