Public Interface IAvatarShipyardModel
    Inherits IAvatarYokedModel
    ReadOnly Property CanChangeEquipment As Boolean
    ReadOnly Property ChangeableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel)
    ReadOnly Property CanInstallEquipment As Boolean
    ReadOnly Property InstallableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel)
    ReadOnly Property CanUninstallEquipment As Boolean
    ReadOnly Property UninstallableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel)
End Interface
