Public Interface IAvatarInventoryItemStackModel
    ReadOnly Property ItemTypeName As String
    ReadOnly Property Count As Integer
    ReadOnly Property Description As String
    ReadOnly Property CanUse As Boolean
    Sub Use()
    ReadOnly Property Items As IEnumerable(Of IItemModel)
    ReadOnly Property Substacks As IEnumerable(Of IAvatarInventoryItemSubstackModel)
End Interface
