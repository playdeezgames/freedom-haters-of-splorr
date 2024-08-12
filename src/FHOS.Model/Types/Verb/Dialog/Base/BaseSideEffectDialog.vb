Imports FHOS.Data

Friend MustInherit Class BaseSideEffectDialog
    Inherits BaseDialog

    Protected Sub New(dialogType As DialogType, actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(dialogType, actor, finalDialog)
    End Sub
End Class
