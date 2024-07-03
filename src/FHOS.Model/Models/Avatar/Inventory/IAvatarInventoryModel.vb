Public Interface IAvatarInventoryModel
    ReadOnly Property Summary As IEnumerable(Of (Text As String, Item As String))
    ReadOnly Property ItemStacks As IEnumerable(Of IAvatarInventoryItemStackModel)
End Interface
