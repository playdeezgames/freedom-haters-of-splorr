Friend Class PrimaryFuelSupplyEquipSlotDescriptor
    Inherits EquipSlotDescriptor

    Public Sub New()
        MyBase.New(
            EquipSlots.PrimaryFuelSupply,
            True,
            "Fuel Supply",
            FuelSupplyCategory)
    End Sub
End Class
