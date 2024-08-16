Imports FHOS.Data

Friend Class SellConfirmDialog
    Inherits BaseInteractorMenuDialog

    Private ReadOnly itemType As String
    Private ReadOnly quantity As Integer

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  itemType As String,
                  quantity As Integer,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            String.Empty)
        Me.itemType = itemType
        Me.quantity = quantity
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From {
            {DialogChoices.Cancel, AddressOf EndDialog},
            {DialogChoices.Confirm, AddressOf ConfirmSale}
        }
        Return menu
    End Function

    Private Function ConfirmSale() As IDialog
        Dim items = actor.Inventory.ItemsOfType(itemType).Take(quantity)
        For Each item In items
            actor.Inventory.Remove(item)
            item.Recycle()
        Next
        actor.ChangeJools(GetJoolsOffered)
        Return EndDialog()
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String)) From {
            (Hues.LightGray, $"Sell {quantity} {GetOfferName()} for {GetJoolsOffered()} Jools?")
        }
        Return lines
    End Function

    Private Function GetJoolsOffered() As Integer
        Return ItemTypes.Descriptors(itemType).Offer * quantity
    End Function

    Private Function GetOfferName() As String
        Return ItemTypes.Descriptors(itemType).Name
    End Function
End Class
