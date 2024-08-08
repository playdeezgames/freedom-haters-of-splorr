Imports FHOS.Data
Imports FHOS.Persistence

Friend Class FuelRodDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelRod,
            "Fuel Rod",
            price:=20,
            onUse:=AddressOf UseFuelRod)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "You ram this into yer engine in order to fill it with fuel. No, there is nothing sexual about this. Not at all."
        End Get
    End Property

    Private Shared Sub UseFuelRod(actor As IActor, item As IItem)
        Dim lines As New List(Of (String, Integer))
        Dim store = actor.FuelTank
        Dim fuelAmount = item.Statistics(StatisticTypes.Fuel).Value
        store.CurrentValue += fuelAmount
        lines.Add(($"Added {fuelAmount} fuel.", Hues.LightGray))
        lines.Add(($"Fuel is now {store.Percent.Value}%.", Hues.LightGray))
        actor.Inventory.Remove(item)
        item.Recycle()
        actor.Universe.Messages.Add("Replenished Fuel!", lines.ToArray)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Fuel) = 100
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog) From
            {
                {"Refill Fuel", New UseFuelRodDialog(actor, item, finalDialog)}
            }
    End Function
End Class
