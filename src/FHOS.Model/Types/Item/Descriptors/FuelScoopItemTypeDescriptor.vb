Imports FHOS.Persistence

Friend Class FuelScoopItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelScoop,
            "Fuel Scoop",
            "This device allows a vessel to replenish fuel directly from a star.",
            equipSlot:=EquipSlots.Accessory,
            price:=500,
            installFee:=20,
            uninstallFee:=10)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefuel) = True
    End Sub
End Class
