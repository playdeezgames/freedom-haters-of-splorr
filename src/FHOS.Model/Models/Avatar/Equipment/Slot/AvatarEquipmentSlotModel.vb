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

    Public ReadOnly Property InstallableItems As IEnumerable(Of IItemModel) Implements IAvatarEquipmentSlotModel.InstallableItems
        Get
            Return actor.
                Inventory.
                Items.
                Where(Function(y) y.Descriptor.CanEquip(equipSlot)).
                Select(AddressOf ItemModel.FromItem)
        End Get
    End Property

    Public ReadOnly Property HasItem As Boolean Implements IAvatarEquipmentSlotModel.HasItem
        Get
            Return actor.Equipment.GetSlot(equipSlot) IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property UninstallFee As Integer Implements IAvatarEquipmentSlotModel.UninstallFee
        Get
            Return If(actor.Equipment.GetSlot(equipSlot)?.Descriptor?.UninstallFee, 0)
        End Get
    End Property

    Public Sub Equip(itemModel As IItemModel) Implements IAvatarEquipmentSlotModel.Equip
        If itemModel Is Nothing Then
            Throw New ArgumentNullException(NameOf(itemModel))
        End If
        If actor.Equipment.GetSlot(equipSlot) IsNot Nothing Then
            Throw New InvalidOperationException("Cannot equip to an already equipped slot.")
        End If
        Dim item = Model.ItemModel.GetItem(itemModel)
        actor.Inventory.Remove(item)
        actor.Equipment.Equip(
            equipSlot,
            item)
        actor.ChangeJools(-item.Descriptor.InstallFee)
        item.OnEquip(actor)
    End Sub

    Friend Shared Function FromActorAndSlot(actor As IActor, equipSlot As String) As IAvatarEquipmentSlotModel
        Return New AvatarEquipmentSlotModel(actor, equipSlot)
    End Function

    Public Function Unequip() As IItemModel Implements IAvatarEquipmentSlotModel.Unequip
        Dim item = actor.Equipment.GetSlot(equipSlot)
        actor.Equipment.Equip(equipSlot, Nothing)
        item.OnUnequip(actor)
        actor.Inventory.Add(item)
        actor.ChangeJools(-item.Descriptor.UninstallFee)
        Return ItemModel.FromItem(item)
    End Function
End Class
