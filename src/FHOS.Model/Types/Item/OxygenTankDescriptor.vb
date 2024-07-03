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
        Const oxygenAmount = 100
        store.CurrentValue += oxygenAmount
        lines.Add(($"Added {oxygenAmount} O2.", Hues.Black))
        lines.Add(($"O2 is now {store.Percent.Value}%.", Hues.Black))
        actor.Inventory.Remove(item)
        item.Recycle()
        actor.Universe.Messages.Add("Replenished Oxygen!", lines.ToArray)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Oxygen) = 50
    End Sub
End Class
