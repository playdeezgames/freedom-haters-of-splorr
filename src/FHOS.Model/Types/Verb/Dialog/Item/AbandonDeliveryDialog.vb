Imports FHOS.Data

Friend Class AbandonDeliveryDialog
    Inherits BaseMenuDialog

    Private ReadOnly item As Persistence.IItem

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
        Me.item = item
    End Sub

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

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From
                {
                    {DialogChoices.Cancel, AddressOf EndDialog},
                    {DialogChoices.Confirm, AddressOf ConfirmAbandon}
                }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim result As New List(Of (Hue As Integer, Text As String)) From {
                (Hues.Orange, item.EntityName),
                (Hues.Red, $"Consider this carefully. This will result in a reputation penalty of {item.GetReputationPenalty}.")
            }
        Return result
    End Function
End Class
