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

    Public ReadOnly Property ItemName As String Implements IAvatarInventoryItemStackModel.ItemName
        Get
            Return Descriptor.Name
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IAvatarInventoryItemStackModel.Count
        Get
            Return actor.Inventory.Items.Count(Function(x) x.Descriptor.ItemType = itemType)
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IAvatarInventoryItemStackModel.Description
        Get
            Return Descriptor.Description
        End Get
    End Property

    Public ReadOnly Property CanUse As Boolean Implements IAvatarInventoryItemStackModel.CanUse
        Get
            Return Descriptor.CanUse
        End Get
    End Property

    Public ReadOnly Property Items As IEnumerable(Of IItemModel) Implements IAvatarInventoryItemStackModel.Items
        Get
            Return actor.Inventory.Items.Where(Function(x) x.Descriptor.ItemType = itemType).Select(AddressOf ItemModel.FromItem)
        End Get
    End Property

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Friend Shared Function FromActorAndItems(actor As IActor, itemType As String) As IAvatarInventoryItemStackModel
        Return New AvatarInventoryItemStackModel(actor, itemType)
    End Function

    Public Sub Use() Implements IAvatarInventoryItemStackModel.Use
        Descriptor.Use(actor, actor.Inventory.Items.First(Function(x) x.EntityType = itemType))
    End Sub
End Class
