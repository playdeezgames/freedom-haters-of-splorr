Imports FHOS.Persistence

Friend Class FuelSupplyItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New(mark As String)
        MyBase.New(
            $"{ItemTypes.FuelSupply}{mark}",
            $"Fuel Supply {Marks.Descriptors(mark).Name}",
            $"This is the Fuel Supply {Marks.Descriptors(mark).Name}. You put fuel in it to make yer ship go.",
            equipSlots:=New Dictionary(Of String, Integer) From
            {
                {Model.EquipSlots.FuelSupply, 1}
            },
            onEquip:=AddressOf EquipFuelSupplyItem)
    End Sub

    Private Shared Sub EquipFuelSupplyItem(actor As IActor, item As IItem)
        actor.Yokes.Store(YokeTypes.FuelTank) = actor.Universe.Factory.CreateStore(item.Statistics(StatisticTypes.FuelCapacity).Value, minimum:=0, maximum:=item.Statistics(StatisticTypes.FuelCapacity))
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.FuelCapacity) = 250
    End Sub
End Class
