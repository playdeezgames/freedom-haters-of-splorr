Imports FHOS.Persistence

Friend Class ActorEquipmentSlotModel
    Implements IActorEquipmentSlotModel

    Private ReadOnly actor As IActor
    Private ReadOnly equipSlot As String

    Public Sub New(actor As IActor, equipSlot As String)
        Me.actor = actor
        Me.equipSlot = equipSlot
    End Sub

    Public ReadOnly Property SlotName As String Implements IActorEquipmentSlotModel.SlotName
        Get
            Return Model.EquipSlots.Descriptors(equipSlot).DisplayName
        End Get
    End Property

    Public ReadOnly Property InstallableItems As IEnumerable(Of IItemModel) Implements IActorEquipmentSlotModel.InstallableItems
        Get
            Return actor.
                Inventory.
                Items.
                Where(Function(y) y.Descriptor.EquipSlot = equipSlot).
                Select(AddressOf ItemModel.FromItem)
        End Get
    End Property

    Public ReadOnly Property UninstallFee As Integer Implements IActorEquipmentSlotModel.UninstallFee
        Get
            Return actor.Equipment.GetSlot(equipSlot).Descriptor.UninstallFee
        End Get
    End Property

    Public Sub Equip(itemModel As IItemModel) Implements IActorEquipmentSlotModel.Equip
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
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= item.Descriptor.InstallFee
        item.OnEquip(actor)
    End Sub

    Friend Shared Function FromActorAndSlot(actor As IActor, equipSlot As String) As IActorEquipmentSlotModel
        Return New ActorEquipmentSlotModel(actor, equipSlot)
    End Function

    Public Function Unequip() As IItemModel Implements IActorEquipmentSlotModel.Unequip
        Dim item = actor.Equipment.GetSlot(equipSlot)
        actor.Equipment.Equip(equipSlot, Nothing)
        item.OnUnequip(actor)
        actor.Inventory.Add(item)
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= item.Descriptor.UninstallFee
        Return ItemModel.FromItem(item)
    End Function
End Class
