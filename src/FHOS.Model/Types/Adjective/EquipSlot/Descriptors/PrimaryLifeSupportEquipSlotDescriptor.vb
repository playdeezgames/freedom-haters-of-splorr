Friend Class PrimaryLifeSupportEquipSlotDescriptor
    Inherits EquipSlotDescriptor
    Public Sub New()
        MyBase.New(
            EquipSlots.PrimaryLifeSupport,
            True,
            "Life Support",
            LifeSupportCategory)
    End Sub
End Class
