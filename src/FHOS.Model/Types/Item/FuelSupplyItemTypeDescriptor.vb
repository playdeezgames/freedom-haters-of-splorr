Imports FHOS.Persistence

Friend Class FuelSupplyItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Private Const FuelCapacityPerMarkValue As Integer = 250
    Private ReadOnly Property fuelCapacity As Integer

    Public Sub New(mark As String)
        MyBase.New(
            $"{ItemTypes.FuelSupply}{mark}",
            $"StarLume Fuel {Marks.Descriptors(mark).Name}",
            $"This is the StarLume Fuel {Marks.Descriptors(mark).Name} from Celestial Energy Solutions. Embark on interstellar journeys with StarLume Fuel by Celestial Energy Solutions, the foremost name in propulsion innovation. Crafted from rare celestial minerals and refined through cutting-edge fusion technology, StarLume Fuel guarantees unmatched efficiency and reliability for your spacecraft. Whether you're charting new frontiers or navigating through asteroid belts, trust Celestial Energy Solutions to propel you farther and faster than ever before. Reach for the stars with StarLume Fuel—where limitless possibilities await beyond every horizon.",
            equipSlots:=New Dictionary(Of String, Integer) From
            {
                {Model.EquipSlots.FuelSupply, 1}
            },
            onEquip:=AddressOf EquipFuelSupplyItem)
        Me.fuelCapacity = Marks.Descriptors(mark).Value * FuelCapacityPerMarkValue
    End Sub

    Private Shared Sub EquipFuelSupplyItem(actor As IActor, item As IItem)
        actor.Yokes.Store(YokeTypes.FuelTank) = actor.Universe.Factory.CreateStore(item.Statistics(StatisticTypes.FuelCapacity).Value, minimum:=0, maximum:=item.Statistics(StatisticTypes.FuelCapacity))
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.FuelCapacity) = fuelCapacity
    End Sub
End Class
