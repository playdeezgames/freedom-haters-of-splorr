Imports FHOS.Data

Friend Class UseFuelRodDialog
    Inherits BaseDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing
    Private ReadOnly item As Persistence.IItem

    Public Sub New(
                  actor As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog, Nothing)
        Me.item = item
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

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From
                {
                    {DialogChoices.Ok, AddressOf EndDialog}
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Menu As IEnumerable(Of String)
        Get
            Return LegacyChoices.Keys
        End Get
    End Property

    Public Overrides Function Choose(choice As String) As IDialog
        Dim value As Func(Of IDialog) = Nothing
        If LegacyChoices().TryGetValue(choice, value) Then
            Return value()
        End If
        Return Me
    End Function
End Class
