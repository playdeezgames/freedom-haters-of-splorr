Friend Class PrimaryLifeSupportEquipSlotDescriptor
    Inherits EquipSlotDescriptor
    Public Sub New()
        MyBase.New(
            EquipSlots.PrimaryLifeSupport,
            "Life Support",
            LifeSupportCategory)
    End Sub
End Class
