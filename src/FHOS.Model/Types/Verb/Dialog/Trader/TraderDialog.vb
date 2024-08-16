Imports FHOS.Data

Friend Class TraderDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            "Now What?")
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From {
            {DialogChoices.Cancel, AddressOf EndDialog}
        }
        If interactor.HasPrices Then
            menu.Add(DialogChoices.Buy, AddressOf SeePrices)
        End If
        If interactor.HasOffers(Actor) Then
            menu.Add(DialogChoices.Sell, AddressOf SeeOffers)
        End If
        Return menu
    End Function

    Private Function SeeOffers() As IDialog
        Reset()
        Return New OffersDialog(actor, interactor, Me)
    End Function

    Private Function SeePrices() As IDialog
        Reset()
        Return New PricesDialog(actor, interactor, Me)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String)) From {
            (Hues.Orange, interactor.EntityName)
        }
        lines.Add((Hues.LightGray, $"Jools: {actor.GetJools}"))
        Return lines
    End Function
End Class
