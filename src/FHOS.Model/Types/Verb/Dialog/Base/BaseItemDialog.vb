Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseItemDialog
    Inherits BaseSideEffectDialog

    Protected Sub New(
                     dialogType As DialogType,
                     actor As IActor,
                     item As IItem,
                     finalDialog As IDialog)
        MyBase.New(dialogType, actor, finalDialog)
        Me.Item = item
    End Sub

    Protected ReadOnly Property Item As IItem
End Class
