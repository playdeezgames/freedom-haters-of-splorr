Imports FHOS.Persistence

Friend Class FuelScoopItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelScoop,
            "Fuel Scoop",
            equipSlot:=EquipSlots.Accessory,
            price:=10000,
            installFee:=100,
            uninstallFee:=50)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "This device allows a vessel to replenish fuel directly from a star."
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefuel) = True
    End Sub
End Class
