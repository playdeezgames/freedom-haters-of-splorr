Public Interface IAvatarTraderModel
    ReadOnly Property IsActive As Boolean
    Sub Leave()
    ReadOnly Property HasOffers As Boolean
    ReadOnly Property Offers As IEnumerable(Of IAvatarTraderOfferModel)
    ReadOnly Property HasPrices As Boolean
    ReadOnly Property Prices As IEnumerable(Of IAvatarTraderPriceModel)
    ReadOnly Property Specimen As IActorModel
End Interface
