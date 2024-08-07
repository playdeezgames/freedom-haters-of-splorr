﻿Imports FHOS.Data

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

    Public Function GetReputation(group As IGroup) As Integer? Implements IActor.GetReputation
        If group Is Nothing Then
            Throw New ArgumentNullException(NameOf(group))
        End If
        Dim reputation As Integer = 0
        If EntityData.Reputations.TryGetValue(group.Id, reputation) Then
            Return reputation
        End If
        Return Nothing
    End Function

    Public Sub SetReputation(group As IGroup, reputation As Integer?) Implements IActor.SetReputation
        If group Is Nothing Then
            Throw New ArgumentNullException(NameOf(group))
        End If
        EntityData.Reputations.Remove(group.Id)
        If reputation.HasValue Then
            EntityData.Reputations.Add(group.Id, reputation.Value)
        End If
    End Sub

    Public Sub AddReputation(group As IGroup, delta As Integer?) Implements IActor.AddReputation
        If delta.HasValue Then
            SetReputation(group, If(GetReputation(group), 0) + delta.Value)
        End If
    End Sub

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

    Public Property Dialog As IDialog Implements IActor.Dialog
        Get
            Return EntityData.Dialog
        End Get
        Set(value As IDialog)
            EntityData.Dialog = value
        End Set
    End Property
End Class
