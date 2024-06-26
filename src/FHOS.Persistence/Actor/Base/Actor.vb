﻿Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Protected Sub New(universeData As Data.IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As IUniverseData, actorId As Integer?) As IActor
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

    Public ReadOnly Property Equipment As IActorEquipment Implements IActor.Equipment
        Get
            Return ActorEquipment.FromId(UniverseData, Id)
        End Get
    End Property

    Public Property Location As ILocation Implements IActor.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.GetStatistic(PersistenceStatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.GetStatistic(PersistenceStatisticTypes.LocationId).Value Then
                Location.Actor = Nothing
                EntityData.SetStatistic(PersistenceStatisticTypes.LocationId, value.Id)
                value.Actor = Actor.FromId(UniverseData, Id)
            End If
        End Set
    End Property

    Public Property Interior As IMap Implements IActor.Interior
        Get
            Return Map.FromId(UniverseData, GetStatistic(PersistenceStatisticTypes.MapId))
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
            Return ActorYokes.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Inventory As IActorInventory Implements IActor.Inventory
        Get
            Return ActorInventory.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Offers As IActorOffers Implements IActor.Offers
        Get
            Return ActorOffers.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Prices As IActorPrices Implements IActor.Prices
        Get
            Return ActorPrices.FromId(UniverseData, Id)
        End Get
    End Property
End Class
