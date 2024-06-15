Public Interface IAvatarTraderModel
    ReadOnly Property IsActive As Boolean
    Sub Leave()
    ReadOnly Property HasOffers As Boolean
    ReadOnly Property HasPrices As Boolean
    ReadOnly Property Trader As IActorModel
End Interface
