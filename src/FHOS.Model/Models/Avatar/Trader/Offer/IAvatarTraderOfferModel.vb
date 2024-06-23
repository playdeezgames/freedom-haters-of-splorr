Public Interface IAvatarTraderOfferModel
    ReadOnly Property Name As String
    ReadOnly Property Quantity As Integer
    Sub Sell(quantity As Integer)
End Interface
