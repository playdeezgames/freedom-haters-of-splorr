Public Interface IAvatarInventoryItemStackModel
    ReadOnly Property ItemTypeName As String
    ReadOnly Property Count As Integer
    ReadOnly Property Items As IEnumerable(Of IItemModel)
    ReadOnly Property Substacks As IEnumerable(Of IAvatarInventoryItemSubstackModel)
End Interface
