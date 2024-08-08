Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseItemDialog
    Inherits BaseDialog

    Protected Sub New(
                     actor As IActor,
                     item As IItem,
                     finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
        Me.Item = item
    End Sub

    Protected ReadOnly Property Item As IItem
End Class
