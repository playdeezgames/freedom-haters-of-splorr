Imports FHOS.Persistence

Friend Class AvatarInventoryItemStackModel
    Implements IAvatarInventoryItemStackModel

    Public ReadOnly Property ItemName As String Implements IAvatarInventoryItemStackModel.ItemName
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IAvatarInventoryItemStackModel.Count
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Friend Shared Function FromActorAndItems(actor As IActor, items As IEnumerable(Of IItem)) As IAvatarInventoryItemStackModel
        Throw New NotImplementedException()
    End Function
End Class
