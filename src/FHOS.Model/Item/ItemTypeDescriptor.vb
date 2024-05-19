Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    Sub New(itemType As String)
        Me.ItemType = itemType
    End Sub
    Function CreateItem(universe As IUniverse) As IItem
        Return universe.Factory.CreateItem(ItemType)
    End Function
End Class
