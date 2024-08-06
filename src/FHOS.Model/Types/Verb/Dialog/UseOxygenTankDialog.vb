Imports FHOS.Data

Friend Class UseOxygenTankDialog
    Inherits BaseDialog
    Private ReadOnly actor As Persistence.IActor
    Private ReadOnly item As Persistence.IItem
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem)
        Me.actor = actor
        Me.item = item
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Replenished Oxygen!")
                    }
                Dim store = actor.LifeSupport
                Dim oxygenAmount = item.Statistics(StatisticTypes.Oxygen).Value
                store.CurrentValue += oxygenAmount
                result.Add((Hues.LightGray, $"Added {oxygenAmount} O2."))
                result.Add((Hues.LightGray, $"O2 is now {store.Percent.Value}%."))
                actor.Inventory.Remove(item)
                actor.Inventory.Add(actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
                item.Recycle()
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return {
                ("Ok", Sub() actor.Dialog = Nothing)
                }
        End Get
    End Property
End Class
