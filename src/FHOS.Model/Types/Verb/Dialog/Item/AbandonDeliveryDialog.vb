Imports FHOS.Data

Friend Class AbandonDeliveryDialog
    Inherits BaseItemDialog

    Public Sub New(actor As Persistence.IActor, item As Persistence.IItem, finalDialog As IDialog)
        MyBase.New(actor, item, finalDialog)
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

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
        Get
            Return New List(Of (Text As String, Value As Action)) From
                {
                    (DialogChoices.Cancel, AddressOf EndDialog),
                    (DialogChoices.Confirm, AddressOf ConfirmAbandon)
                }
        End Get
    End Property

    Private Sub ConfirmAbandon()
        Dim delta = Item.GetReputationPenalty
        Actor.UpdateReputations(
            delta,
            Item.GetOriginPlanet,
            Actor.UpdateReputations(delta, Item.GetDestinationPlanet))
        Actor.Inventory.Remove(Item)
        Item.Recycle()
        EndDialog()
    End Sub
End Class
