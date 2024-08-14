Imports FHOS.Data

Friend Class EnterShipyardDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            interactor.EntityName)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        Return menu
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
