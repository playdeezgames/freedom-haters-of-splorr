Imports FHOS.Data

Friend MustInherit Class BaseMenuDialog
    Inherits BaseDialog

    Protected Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog, Nothing, Nothing)
    End Sub
End Class
