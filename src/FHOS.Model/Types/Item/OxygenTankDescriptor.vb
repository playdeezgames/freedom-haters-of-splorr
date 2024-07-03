Imports FHOS.Persistence

Friend Class OxygenTankDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(ItemTypes.OxygenTank, "Oxygen Tank", "This item can be used to replenish a vessel's oxygen.", price:=5)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Oxygen) = 50
    End Sub
End Class
