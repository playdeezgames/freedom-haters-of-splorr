Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    Sub New(itemType As String)
        Me.ItemType = itemType
    End Sub
    Function CreateItem(universe As IUniverse) As IItem
        Dim item = universe.Factory.CreateItem(ItemType)
        Initialize(item)
        Return item
    End Function
    Protected MustOverride Sub Initialize(item As IItem)
End Class
