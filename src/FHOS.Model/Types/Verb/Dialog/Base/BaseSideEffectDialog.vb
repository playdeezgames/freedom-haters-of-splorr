Imports FHOS.Data

Friend MustInherit Class BaseSideEffectDialog
    Inherits BaseDialog

    Protected Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub
End Class
