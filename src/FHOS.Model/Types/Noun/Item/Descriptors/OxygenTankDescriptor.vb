Imports FHOS.Data
Imports FHOS.Persistence

Friend Class OxygenTankDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.OxygenTank,
            "Oxygen Tank",
            price:=5,
            onUse:=AddressOf UseOxygenTank)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "This item can be used to replenish a vessel's oxygen."
        End Get
    End Property

    Private Shared Sub UseOxygenTank(actor As IActor, item As IItem)
        Dim lines As New List(Of (String, Integer))
        Dim store = actor.LifeSupport
        Dim oxygenAmount = item.Statistics(StatisticTypes.Oxygen).Value
        store.CurrentValue += oxygenAmount
        lines.Add(($"Added {oxygenAmount} O2.", Hues.LightGray))
        lines.Add(($"O2 is now {store.Percent.Value}%.", Hues.LightGray))
        actor.Inventory.Remove(item)
        actor.Inventory.Add(actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
        item.Recycle()
        actor.Universe.Messages.Add("Replenished Oxygen!", lines.ToArray)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Oxygen) = 100
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog) From
            {
                {"Refill Oxygen", New UseOxygenTankDialog(actor, item, Nothing)}
            }
    End Function
End Class
