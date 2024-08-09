Friend Module EquipSlots
    Friend ReadOnly Accessory As String = NameOf(Accessory)
    Friend ReadOnly PrimaryFuelSupply As String = NameOf(PrimaryFuelSupply)
    Friend ReadOnly PrimaryLifeSupport As String = NameOf(PrimaryLifeSupport)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, EquipSlotDescriptor) =
        New List(Of EquipSlotDescriptor) From
        {
            New PrimaryLifeSupportEquipSlotDescriptor(),
            New PrimaryFuelSupplyEquipSlotDescriptor(),
            New AccessoryEquipSlotDescriptor()
        }.ToDictionary(Function(x) x.EquipSlot, Function(x) x)
End Module
