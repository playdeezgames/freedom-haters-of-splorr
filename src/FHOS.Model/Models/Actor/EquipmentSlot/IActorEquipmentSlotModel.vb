Public Interface IActorEquipmentSlotModel
    ReadOnly Property SlotName As String
    ReadOnly Property InstallableItems As IEnumerable(Of IItemModel)
    Function Unequip() As IItemModel
    Sub Equip(item As IItemModel)
End Interface
