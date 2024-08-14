Imports FHOS.Data

Friend Class UninstallEquipmentDialog
    Inherits BaseInteractorMenuDialog
    Implements IDialog

    Public Sub New(actor As Persistence.IActor, interactor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, interactor, finalDialog, String.Empty)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Throw New NotImplementedException()
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Throw New NotImplementedException()
    End Function
End Class
