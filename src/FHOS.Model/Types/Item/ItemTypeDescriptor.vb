Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    ReadOnly Property Name As String
    ReadOnly Property Offer As Integer
    ReadOnly Property Price As Integer
    Sub New(itemType As String, name As String, Optional offer As Integer = 0, Optional price As Integer = 0)
        Me.ItemType = itemType
        Me.Name = name
        Me.Offer = offer
        Me.Price = price
    End Sub
    Function CreateItem(universe As IUniverse) As IItem
        Dim item = universe.Factory.CreateItem(ItemType)
        Initialize(item)
        Return item
    End Function
    Protected MustOverride Sub Initialize(item As IItem)
End Class
