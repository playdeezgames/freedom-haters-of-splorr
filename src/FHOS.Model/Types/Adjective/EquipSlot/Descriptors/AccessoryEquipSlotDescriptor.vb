Friend Class AccessoryEquipSlotDescriptor
    Inherits EquipSlotDescriptor

    Public Sub New(index As Integer)
        MyBase.New(
            EquipSlots.Accessory(index),
            False,
            $"Accessory({index})",
            AccessoryCategory)
    End Sub
End Class
