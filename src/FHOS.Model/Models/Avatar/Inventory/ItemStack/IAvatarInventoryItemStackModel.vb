Public Interface IAvatarInventoryItemStackModel
    ReadOnly Property ItemName As String
    ReadOnly Property Count As Integer
    ReadOnly Property Description As String
    ReadOnly Property CanUse As Boolean
    Sub Use()
End Interface
