Imports FHOS.Persistence

Friend Class FuelRodDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelRod,
            "Fuel Rod",
            "You ram this into yer engine in order to fill it with fuel. No, there is nothing sexual about this. Not at all.",
            price:=20,
            onUse:=AddressOf UseFuelRod)
    End Sub

    Private Shared Sub UseFuelRod(actor As IActor, item As IItem)
        Dim lines As New List(Of (String, Integer))
        Dim store = actor.Yokes.Store(YokeTypes.FuelTank)
        Dim fuelAmount = item.Statistics(StatisticTypes.Fuel).Value
        store.CurrentValue += fuelAmount
        lines.Add(($"Added {fuelAmount} fuel.", Hues.Black))
        lines.Add(($"Fuel is now {store.Percent.Value}%.", Hues.Black))
        actor.Inventory.Remove(item)
        item.Recycle()
        actor.Universe.Messages.Add("Replenished Fuel!", lines.ToArray)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Fuel) = 100
    End Sub
End Class
