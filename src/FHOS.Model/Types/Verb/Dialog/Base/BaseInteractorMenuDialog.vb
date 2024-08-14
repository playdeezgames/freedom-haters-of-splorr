Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseInteractorMenuDialog
    Inherits BaseMenuDialog

    Protected Sub New(
                     actor As IActor,
                     interactor As IActor,
                     finalDialog As IDialog,
                     prompt As String)
        MyBase.New(actor, finalDialog, prompt)
        Me.interactor = interactor
    End Sub

    Protected ReadOnly interactor As IActor
End Class
