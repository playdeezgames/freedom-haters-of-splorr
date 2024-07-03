Imports FHOS.Persistence

Friend Class AvatarInventoryItemStackModel
    Implements IAvatarInventoryItemStackModel

    Public Sub New(actor As IActor, itemType As String)
        Me.actor = actor
        Me.itemType = itemType
    End Sub

    Public ReadOnly Property ItemName As String Implements IAvatarInventoryItemStackModel.ItemName
        Get
            Return ItemTypes.Descriptors(itemType).Name
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IAvatarInventoryItemStackModel.Count
        Get
            Return actor.Inventory.Items.Count(Function(x) x.Descriptor.ItemType = itemType)
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IAvatarInventoryItemStackModel.Description
        Get
            Return ItemTypes.Descriptors(itemType).Description
        End Get
    End Property

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Friend Shared Function FromActorAndItems(actor As IActor, itemType As String) As IAvatarInventoryItemStackModel
        Return New AvatarInventoryItemStackModel(actor, itemType)
    End Function
End Class
