Friend Class UseFuelRodDialog
    Inherits BaseItemDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem)
        MyBase.New(actor, item)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Replenished Fuel!")
                    }
                Dim store = actor.FuelTank
                Dim fuelAmount = item.Statistics(StatisticTypes.Fuel).Value
                store.CurrentValue += fuelAmount
                result.Add((Hues.LightGray, $"Added {fuelAmount} fuel."))
                result.Add((Hues.LightGray, $"Fuel is now {store.Percent.Value}%."))
                actor.Inventory.Remove(item)
                item.Recycle()
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return New List(Of (Text As String, Value As Action)) From
                {
                    ("Ok", Sub() actor.Dialog = Nothing)
                }
        End Get
    End Property
End Class
