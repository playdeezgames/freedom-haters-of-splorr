Imports FHOS.Persistence

Friend Class OxygenTankDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.OxygenTank,
            "Oxygen Tank",
            "This item can be used to replenish a vessel's oxygen.",
            price:=5,
            onUse:=AddressOf UseOxygenTank)
    End Sub

    Private Shared Sub UseOxygenTank(actor As IActor, item As IItem)
        Dim lines As New List(Of (String, Integer))
        Dim store = actor.Yokes.Store(YokeTypes.LifeSupport)
        Dim oxygenAmount = item.Statistics(StatisticTypes.Oxygen).Value
        store.CurrentValue += oxygenAmount
        lines.Add(($"Added {oxygenAmount} O2.", Hues.Black))
        lines.Add(($"O2 is now {store.Percent.Value}%.", Hues.Black))
        actor.Inventory.Remove(item)
        actor.Inventory.Add(actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
        item.Recycle()
        actor.Universe.Messages.Add("Replenished Oxygen!", lines.ToArray)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Oxygen) = 100
    End Sub
End Class
