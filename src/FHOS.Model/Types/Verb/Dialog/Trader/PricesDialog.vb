Imports FHOS.Data

Friend Class PricesDialog
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
        Return menu
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String)) From {
            (Hues.Orange, interactor.EntityName)
        }
        lines.Add((Hues.LightGray, $"Jools: {actor.GetJools}"))
        Return lines
    End Function
End Class
