﻿Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Implements IAvatarTraderModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarTraderModel.IsActive
        Get
            Return actor.Yokes.Actor(YokeTypes.Trader) IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property HasOffers As Boolean Implements IAvatarTraderModel.HasOffers
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property HasPrices As Boolean Implements IAvatarTraderModel.HasPrices
        Get
            Return False
        End Get
    End Property

    Private ReadOnly Property Trader As IActorModel Implements IAvatarTraderModel.Trader
        Get
            Return ActorModel.FromActor(actor.Yokes.Actor(YokeTypes.Trader))
        End Get
    End Property

    Public Sub Leave() Implements IAvatarTraderModel.Leave
        actor.Yokes.Actor(YokeTypes.Trader) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function
End Class