Imports FHOS.Data

Friend Class UseOxygenTankDialog
    Inherits BaseItemDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(
                  actor As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(actor, item, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Replenished Oxygen!")
                    }
                Dim store = Actor.LifeSupport
                Dim oxygenAmount = Item.Statistics(StatisticTypes.Oxygen).Value
                store.CurrentValue += oxygenAmount
                result.Add((Hues.LightGray, $"Added {oxygenAmount} O2."))
                result.Add((Hues.LightGray, $"O2 is now {store.Percent.Value}%."))
                Actor.Inventory.Remove(Item)
                Actor.Inventory.Add(Actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
                Item.Recycle()
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Func(Of IDialog)))
        Get
            Return {
                (DialogChoices.Ok, AddressOf EndDialog)
                }
        End Get
    End Property
End Class
