Public Interface IAvatarTraderPriceModel
    ReadOnly Property Name As String
    ReadOnly Property CanBuy As Boolean
    ReadOnly Property MaximumQuantity As Integer
    Sub Buy(quantity As Integer)
End Interface
