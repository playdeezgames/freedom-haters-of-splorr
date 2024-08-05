Imports FHOS.Persistence

Friend Class AvatarInventoryItemStackModel
    Implements IAvatarInventoryItemStackModel

    Public Sub New(actor As IActor, itemType As String)
        Me.actor = actor
        Me.itemType = itemType
    End Sub
    Private ReadOnly Property Descriptor As ItemTypeDescriptor
        Get
            Return ItemTypes.Descriptors(itemType)
        End Get
    End Property

    Public ReadOnly Property ItemTypeName As String Implements IAvatarInventoryItemStackModel.ItemTypeName
        Get
            Return Descriptor.Name
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IAvatarInventoryItemStackModel.Count
        Get
            Return actor.Inventory.ItemsOfType(itemType).Count
        End Get
    End Property

    Public ReadOnly Property Items As IEnumerable(Of IItemModel) Implements IAvatarInventoryItemStackModel.Items
        Get
            Return actor.Inventory.ItemsOfType(itemType).Select(AddressOf ItemModel.FromItem)
        End Get
    End Property

    Public ReadOnly Property Substacks As IEnumerable(Of IAvatarInventoryItemSubstackModel) Implements IAvatarInventoryItemStackModel.Substacks
        Get
            Return actor.
                Inventory.
                ItemsOfType(itemType).
                GroupBy(Function(x) x.EntityName).
                Select(Function(x) AvatarInventoryItemSubstackModel.FromActorAndItems(actor, x.ToList))
        End Get
    End Property

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Friend Shared Function FromActorAndItemType(actor As IActor, itemType As String) As IAvatarInventoryItemStackModel
        Return New AvatarInventoryItemStackModel(actor, itemType)
    End Function
End Class
