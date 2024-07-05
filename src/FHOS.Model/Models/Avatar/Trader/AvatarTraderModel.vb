Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Implements IAvatarTraderModel

    Private ReadOnly actor As IActor
    Private ReadOnly Property YokedTrader As IActor
        Get
            Return actor.Yokes.Actor(YokeTypes.Trader)
        End Get
    End Property


    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarTraderModel.IsActive
        Get
            Return YokedTrader IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property HasOffers As Boolean Implements IAvatarTraderModel.HasOffers
        Get
            Return If(YokedTrader?.Offers?.HasAny(actor), False)
        End Get
    End Property

    Public ReadOnly Property HasPrices As Boolean Implements IAvatarTraderModel.HasPrices
        Get
            Return If(YokedTrader?.Prices?.HasAny, False)
        End Get
    End Property

    Private ReadOnly Property Trader As IActorModel Implements IAvatarTraderModel.Trader
        Get
            Return ActorModel.FromActor(YokedTrader)
        End Get
    End Property

    Public ReadOnly Property Offers As IEnumerable(Of IAvatarTraderOfferModel) Implements IAvatarTraderModel.Offers
        Get
            Return If(YokedTrader?.Offers?.All(actor).Select(Function(x) AvatarTraderOfferModel.FromActorOffer(actor, x)), Array.Empty(Of IAvatarTraderOfferModel))
        End Get
    End Property

    Public ReadOnly Property Prices As IEnumerable(Of IAvatarTraderPriceModel) Implements IAvatarTraderModel.Prices
        Get
            Return If(YokedTrader?.Prices?.All.Select(Function(x) AvatarTraderPriceModel.FromActorPrice(actor, x)), Array.Empty(Of IAvatarTraderPriceModel))
        End Get
    End Property

    Public Sub Leave() Implements IAvatarTraderModel.Leave
        actor.Yokes.Actor(YokeTypes.Trader) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function
End Class
