Friend Module EquipSlots
    Friend ReadOnly Property Accessory(index As Integer) As String
        Get
            Return $"Accessory{index}"
        End Get
    End Property
    Friend ReadOnly PrimaryFuelSupply As String = NameOf(PrimaryFuelSupply)
    Friend ReadOnly PrimaryLifeSupport As String = NameOf(PrimaryLifeSupport)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, EquipSlotDescriptor) =
        New List(Of EquipSlotDescriptor) From
        {
            New PrimaryLifeSupportEquipSlotDescriptor(),
            New PrimaryFuelSupplyEquipSlotDescriptor(),
            New AccessoryEquipSlotDescriptor(0),
            New AccessoryEquipSlotDescriptor(1)
        }.ToDictionary(Function(x) x.EquipSlot, Function(x) x)
End Module
