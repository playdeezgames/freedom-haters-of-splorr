Imports FHOS.Persistence

Friend Class AtmosphericConcentratorItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.AtmosphericConcentrator,
            "Atmospheric Concentrator",
            "This device allows a vessel to replenish their oxygen from a planet's atmosphere.",
            equipSlot:=EquipSlots.Accessory,
            price:=500,
            installFee:=20,
            uninstallFee:=10) 'TODO: make this not cheap once it works
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefillOxygen) = True
    End Sub
End Class
