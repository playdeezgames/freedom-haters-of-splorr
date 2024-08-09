Friend Class PrimaryFuelSupplyEquipSlotDescriptor
    Inherits EquipSlotDescriptor

    Public Sub New()
        MyBase.New(
            EquipSlots.PrimaryFuelSupply,
            "Fuel Supply",
            FuelSupplyCategory)
    End Sub
End Class
