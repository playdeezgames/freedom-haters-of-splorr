Imports FHOS.Persistence

Friend Class AvatarInventoryItemSubstackModel
    Implements IAvatarInventoryItemSubstackModel

    Public Sub New(actor As IActor, items As IEnumerable(Of IItem))
        Me.Actor = actor
        Me.Items = items
    End Sub

    Public ReadOnly Property EntityName As String Implements IAvatarInventoryItemSubstackModel.EntityName
        Get
            Return items.First.EntityName
        End Get
    End Property

    'is this needed?
    Private ReadOnly Property actor As IActor
    Private ReadOnly Property items As IEnumerable(Of IItem)

    Private ReadOnly Property IAvatarInventoryItemSubstackModel_Items As IEnumerable(Of IItemModel) Implements IAvatarInventoryItemSubstackModel.Items
        Get
            Return items.Select(AddressOf ItemModel.FromItem)
        End Get
    End Property

    Friend Shared Function FromActorAndItems(actor As IActor, items As IEnumerable(Of IItem)) As IAvatarInventoryItemSubstackModel
        Return New AvatarInventoryItemSubstackModel(actor, items)
    End Function
End Class
