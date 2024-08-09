Friend Class AccessoryEquipSlotDescriptor
    Inherits EquipSlotDescriptor

    Public Sub New()
        MyBase.New(
            EquipSlots.Accessory,
            "Accessory",
            AccessoryCategory)
    End Sub
End Class
