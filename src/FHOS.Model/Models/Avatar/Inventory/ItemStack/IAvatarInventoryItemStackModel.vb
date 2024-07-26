Public Interface IAvatarInventoryItemStackModel
    ReadOnly Property ItemName As String
    ReadOnly Property Count As Integer
    ReadOnly Property Description As String
    ReadOnly Property CanUse As Boolean
    Sub Use()
    ReadOnly Property Items As IEnumerable(Of IItemModel)
End Interface
