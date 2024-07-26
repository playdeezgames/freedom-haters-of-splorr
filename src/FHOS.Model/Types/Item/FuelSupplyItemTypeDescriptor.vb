Imports FHOS.Persistence

Friend Class FuelSupplyItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Private Const FuelCapacityPerMarkValue As Integer = 250
    Private Const PricePerMarkValue As Integer = 500
    Private ReadOnly Property fuelCapacity As Integer

    Public Sub New(markType As String)
        MyBase.New(
            ItemTypes.MarkedType(ItemTypes.FuelSupply, markType),
            $"StarLume Fuel {Marks.Descriptors(markType).Name}",
            $"This is the StarLume Fuel {Marks.Descriptors(markType).Name} from Celestial Energy Solutions. Embark on interstellar journeys with StarLume Fuel by Celestial Energy Solutions, the foremost name in propulsion innovation. Crafted from rare celestial minerals and refined through cutting-edge fusion technology, StarLume Fuel guarantees unmatched efficiency and reliability for your spacecraft. Whether you're charting new frontiers or navigating through asteroid belts, trust Celestial Energy Solutions to propel you farther and faster than ever before. Reach for the stars with StarLume Fuel—where limitless possibilities await beyond every horizon.",
            onEquip:=AddressOf EquipFuelSupplyItem,
            onUnequip:=AddressOf UnequipFuelSupplyItem,
            price:=CalculatePrice(markType),
            equipSlot:=EquipSlots.FuelSupply)
        Me.fuelCapacity = Marks.Descriptors(markType).Value * FuelCapacityPerMarkValue
    End Sub

    Private Shared Sub UnequipFuelSupplyItem(actor As IActor, item As IItem)
        Dim store = actor.Yokes.Store(YokeTypes.FuelTank).MaximumValue = 0
    End Sub

    Private Shared Function CalculatePrice(markType As String) As Integer
        Return Marks.Descriptors(markType).Value * PricePerMarkValue
    End Function

    Private Shared Sub EquipFuelSupplyItem(actor As IActor, item As IItem)
        Dim store = actor.Yokes.Store(YokeTypes.FuelTank)
        store.MaximumValue = item.Statistics(StatisticTypes.FuelCapacity)
        store.CurrentValue = If(item.Statistics(StatisticTypes.FuelCapacity), 0)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.FuelCapacity) = fuelCapacity
    End Sub
End Class
