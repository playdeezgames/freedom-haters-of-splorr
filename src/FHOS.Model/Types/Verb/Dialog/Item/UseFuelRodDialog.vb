Imports FHOS.Data

Friend Class UseFuelRodDialog
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
                        (Hues.Orange, "Replenished Fuel!")
                    }
                Dim store = Actor.FuelTank
                Dim fuelAmount = Item.Statistics(StatisticTypes.Fuel).Value
                store.CurrentValue += fuelAmount
                result.Add((Hues.LightGray, $"Added {fuelAmount} fuel."))
                result.Add((Hues.LightGray, $"Fuel is now {store.Percent.Value}%."))
                Actor.Inventory.Remove(Item)
                Item.Recycle()
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return New List(Of (Text As String, Value As Action)) From
                {
                    (DialogChoices.Ok, AddressOf EndDialog)
                }
        End Get
    End Property
End Class
