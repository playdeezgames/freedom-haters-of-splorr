Public Interface IAvatarTraderModel
    Inherits IAvatarYokedModel
    ReadOnly Property HasOffers As Boolean
    ReadOnly Property Offers As IEnumerable(Of IAvatarTraderOfferModel)
    ReadOnly Property HasPrices As Boolean
    ReadOnly Property Prices As IEnumerable(Of IAvatarTraderPriceModel)
End Interface
