Public Interface IAvatarTraderOfferModel
    ReadOnly Property NameAndQuantity As String
    ReadOnly Property Name As String
    ReadOnly Property Quantity As Integer
    Sub Sell(quantity As Integer)
    Function JoolsOffered(quantity As Integer) As Integer
End Interface
