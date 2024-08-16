Imports FHOS.Data

Friend Class OffersDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            String.Empty)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From {
            {DialogChoices.Cancel, AddressOf EndDialog}
        }
        For Each itemType In interactor.Offers.All(actor)
            menu.Add(ToName(itemType), PickOffer(itemType))
        Next
        Return menu
    End Function

    Private Function ToName(itemType As String) As String
        Dim nameAndQuantity = $"{ItemTypes.Descriptors(itemType).Name}(x{actor.Inventory.ItemCountOfType(itemType)})"
        Dim joolsOffered = ItemTypes.Descriptors(itemType).Offer
        Return $"{nameAndQuantity}@{joolsOffered}"
    End Function

    Private Function PickOffer(itemType As String) As Func(Of IDialog)
        Return Function() New SellQuantityDialog(actor, interactor, itemType, finalDialog)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String)) From {
            (Hues.Orange, interactor.EntityName),
            (Hues.LightGray, $"Jools: {actor.GetJools}")
        }
        Return lines
    End Function
End Class
