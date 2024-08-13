Imports FHOS.Data

Friend Class AbandonDeliveryDialog
    Inherits BaseDialog

    Private ReadOnly item As Persistence.IItem

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem, finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog, Nothing)
        Me.item = item
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Orange, Item.EntityName),
                (Hues.Red, $"Consider this carefully. This will result in a reputation penalty of {Item.GetReputationPenalty}.")
            }
            Return result
        End Get
    End Property

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From
                {
                    {DialogChoices.Cancel, AddressOf EndDialog},
                    {DialogChoices.Confirm, AddressOf ConfirmAbandon}
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

    Private Function ConfirmAbandon() As IDialog
        Dim delta = Item.GetReputationPenalty
        Actor.UpdateReputations(
            delta,
            Item.GetOriginPlanet,
            Actor.UpdateReputations(delta, Item.GetDestinationPlanet))
        Actor.Inventory.Remove(Item)
        Item.Recycle()
        Return EndDialog()
    End Function
End Class
