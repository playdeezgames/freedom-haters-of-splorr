Friend Module EquipSlots
    Friend ReadOnly Accessory As String = NameOf(Accessory)
    Friend ReadOnly FuelSupply As String = NameOf(FuelSupply)
    Friend ReadOnly LifeSupport As String = NameOf(LifeSupport)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, EquipSlotDescriptor) =
        New List(Of EquipSlotDescriptor) From
        {
            New LifeSupportEquipSlotDescriptor(),
            New FuelSupplyEquipSlotDescriptor(),
            New AccessoryEquipSlotDescriptor()
        }.ToDictionary(Function(x) x.EquipSlot, Function(x) x)
End Module
