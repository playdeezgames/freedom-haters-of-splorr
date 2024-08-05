Imports FHOS.Data

Friend Class UseFuelRodDialog
    Implements IDialog
    Private ReadOnly actor As Persistence.IActor
    Private ReadOnly item As Persistence.IItem
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem)
        Me.actor = actor
        Me.item = item
    End Sub

    Public ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String)) Implements IDialog.Lines
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

    Public ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action)) Implements IDialog.Choices
        Get
            Return New List(Of (Text As String, Value As Action)) From
                {
                    ("Ok", Sub() actor.Dialog = Nothing)
                }
        End Get
    End Property
End Class
