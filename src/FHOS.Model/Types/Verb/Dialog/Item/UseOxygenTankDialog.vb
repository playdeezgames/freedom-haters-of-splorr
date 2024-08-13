Imports FHOS.Data

Friend Class UseOxygenTankDialog
    Inherits BaseItemMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(actor, item, finalDialog)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From {
                {DialogChoices.Ok, AddressOf EndDialog}
                }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Replenished Oxygen!")
                    }
        Dim store = Actor.LifeSupport
        Dim oxygenAmount = item.Statistics(StatisticTypes.Oxygen).Value
        store.CurrentValue += oxygenAmount
        result.Add((Hues.LightGray, $"Added {oxygenAmount} O2."))
        result.Add((Hues.LightGray, $"O2 is now {store.Percent.Value}%."))
        Actor.Inventory.Remove(item)
        Actor.Inventory.Add(Actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
        item.Recycle()
        Return result
    End Function
End Class
