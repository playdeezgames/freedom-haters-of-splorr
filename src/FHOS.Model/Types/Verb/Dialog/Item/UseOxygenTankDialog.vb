Imports FHOS.Data

Friend Class UseOxygenTankDialog
    Inherits BaseMenuDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing
    Private ReadOnly item As Persistence.IItem

    Public Sub New(
                  actor As Persistence.IActor,
                  item As Persistence.IItem,
                  finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
        Me.item = item
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

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From {
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
