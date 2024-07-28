Imports FHOS.Persistence

Friend Class FuelScoopItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelScoop,
            "Fuel Scoop",
            "This device allows a vessel to replenish fuel directly from a star.",
            equipSlot:=EquipSlots.Accessory,
            price:=10000,
            installFee:=100,
            uninstallFee:=50)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefuel) = True
    End Sub
End Class
