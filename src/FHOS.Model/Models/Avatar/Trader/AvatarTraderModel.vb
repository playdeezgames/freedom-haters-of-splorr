﻿Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Implements IAvatarTraderModel

    Private ReadOnly actor As IActor
    Private ReadOnly Property yokedTrader As IActor
        Get
            Return actor.Yokes.Actor(YokeTypes.Trader)
        End Get
    End Property


    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarTraderModel.IsActive
        Get
            Return yokedTrader IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property HasOffers As Boolean Implements IAvatarTraderModel.HasOffers
        Get
            Return If(yokedTrader?.Offers?.HasAny(actor), False)
        End Get
    End Property

    Public ReadOnly Property HasPrices As Boolean Implements IAvatarTraderModel.HasPrices
        Get
            Return If(yokedTrader?.Prices?.HasAny, False)
        End Get
    End Property

    Private ReadOnly Property Trader As IActorModel Implements IAvatarTraderModel.Trader
        Get
            Return ActorModel.FromActor(yokedTrader)
        End Get
    End Property

    Public ReadOnly Property Offers As IEnumerable(Of IAvatarTraderOfferModel) Implements IAvatarTraderModel.Offers
        Get
            Return If(yokedTrader?.Offers?.All(actor).Select(Function(x) AvatarTraderOfferModel.FromActorOffer(actor, x)), Array.Empty(Of IAvatarTraderOfferModel))
        End Get
    End Property

    Public ReadOnly Property Prices As IEnumerable(Of IAvatarTraderPriceModel) Implements IAvatarTraderModel.Prices
        Get
            Return If(yokedTrader?.Prices?.All.Select(Function(x) AvatarTraderPriceModel.FromActorPrice(actor, x)), Array.Empty(Of IAvatarTraderPriceModel))
        End Get
    End Property

    Public Sub Leave() Implements IAvatarTraderModel.Leave
        actor.Yokes.Actor(YokeTypes.Trader) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function
End Class
