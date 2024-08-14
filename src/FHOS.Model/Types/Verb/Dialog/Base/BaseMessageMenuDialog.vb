Imports FHOS.Data

Friend MustInherit Class BaseMessageMenuDialog
    Inherits BaseMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog, String.Empty)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From {
                {DialogChoices.Ok, AddressOf EndDialog}
                }
    End Function
End Class
