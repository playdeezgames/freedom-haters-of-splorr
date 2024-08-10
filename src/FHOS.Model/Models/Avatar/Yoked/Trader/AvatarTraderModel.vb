Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Inherits AvatarYokedModel
    Implements IAvatarTraderModel

    Public Sub New(actor As IActor)
        MyBase.New(actor, YokeTypes.Trader)
    End Sub

    Public ReadOnly Property HasOffers As Boolean Implements IAvatarTraderModel.HasOffers
        Get
            Return If(YokedActor?.Offers?.HasAny(actor), False)
        End Get
    End Property

    Public ReadOnly Property HasPrices As Boolean Implements IAvatarTraderModel.HasPrices
        Get
            Return If(YokedActor?.Prices?.HasAny, False)
        End Get
    End Property

    Public ReadOnly Property Offers As IEnumerable(Of IAvatarTraderOfferModel) Implements IAvatarTraderModel.Offers
        Get
            Return If(YokedActor?.Offers?.All(actor).Select(Function(x) AvatarTraderOfferModel.FromActorOffer(actor, x)), Array.Empty(Of IAvatarTraderOfferModel))
        End Get
    End Property

    Public ReadOnly Property Prices As IEnumerable(Of IAvatarTraderPriceModel) Implements IAvatarTraderModel.Prices
        Get
            Return If(
                    YokedActor?.
                    Prices?.
                    All.
                    Select(Function(x) AvatarTraderPriceModel.FromActorPrice(actor, x)
                ),
                Array.Empty(Of IAvatarTraderPriceModel))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub
End Class
