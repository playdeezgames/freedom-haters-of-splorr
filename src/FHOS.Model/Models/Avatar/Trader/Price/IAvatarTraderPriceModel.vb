Public Interface IAvatarTraderPriceModel
    ReadOnly Property Name As String
    ReadOnly Property UnitPrice As Integer
    ReadOnly Property CanBuy As Boolean
    ReadOnly Property MaximumQuantity As Integer
    ReadOnly Property InventoryQuantity As Integer
    Sub Buy(quantity As Integer)
    Function TotalPrice(quantity As Integer) As Integer
End Interface
