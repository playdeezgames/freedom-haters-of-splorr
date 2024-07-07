Friend Module EquipSlots
    Friend ReadOnly LifeSupport As String = NameOf(LifeSupport)
    Friend ReadOnly FuelSupply As String = NameOf(FuelSupply)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, EquipSlotDescriptor) =
        New List(Of EquipSlotDescriptor) From
        {
            New LifeSupportEquipSlotDescriptor(),
            New FuelSupplyEquipSlotDescriptor()
        }.ToDictionary(Function(x) x.EquipSlot, Function(x) x)
End Module
