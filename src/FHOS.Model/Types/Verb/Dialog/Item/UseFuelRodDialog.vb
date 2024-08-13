Imports FHOS.Data

Friend Class UseFuelRodDialog
    Inherits BaseItemMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(actor, item, finalDialog)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From
                {
                    {DialogChoices.Ok, AddressOf EndDialog}
                }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Replenished Fuel!")
                    }
        Dim store = Actor.FuelTank
        Dim fuelAmount = item.Statistics(StatisticTypes.Fuel).Value
        store.CurrentValue += fuelAmount
        result.Add((Hues.LightGray, $"Added {fuelAmount} fuel."))
        result.Add((Hues.LightGray, $"Fuel is now {store.Percent.Value}%."))
        Actor.Inventory.Remove(item)
        item.Recycle()
        Return result
    End Function
End Class
