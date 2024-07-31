Public Interface IAvatarEquipmentSlotModel
    ReadOnly Property SlotName As String
    ReadOnly Property Item As IItemModel
    ReadOnly Property InstallableItems As IEnumerable(Of IItemModel)
    ReadOnly Property HasItem As Boolean
    Function Unequip() As IItemModel
    Sub Equip(itemModel As IItemModel)
    ReadOnly Property UninstallFee As Integer
End Interface
