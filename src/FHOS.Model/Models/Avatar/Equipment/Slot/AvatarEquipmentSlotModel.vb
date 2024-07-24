Imports FHOS.Persistence

Friend Class AvatarEquipmentSlotModel
    Implements IAvatarEquipmentSlotModel

    Private ReadOnly actor As IActor
    Private ReadOnly equipSlot As String

    Public Sub New(actor As IActor, equipSlot As String)
        Me.actor = actor
        Me.equipSlot = equipSlot
    End Sub

    Public ReadOnly Property SlotName As String Implements IAvatarEquipmentSlotModel.SlotName
        Get
            Return EquipSlots.Descriptors(equipSlot).DisplayName
        End Get
    End Property

    Public ReadOnly Property Item As IItemModel Implements IAvatarEquipmentSlotModel.Item
        Get
            Return ItemModel.FromItem(actor.Equipment.GetSlot(equipSlot))
        End Get
    End Property

    Friend Shared Function FromActorAndSlot(actor As IActor, equipSlot As String) As IAvatarEquipmentSlotModel
        Return New AvatarEquipmentSlotModel(actor, equipSlot)
    End Function
End Class
