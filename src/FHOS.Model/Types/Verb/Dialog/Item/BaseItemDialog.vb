Imports FHOS.Persistence

Friend MustInherit Class BaseItemDialog
    Inherits BaseDialog

    Protected Sub New(
                     actor As IActor,
                     item As IItem)
        MyBase.New(actor)
        Me.Item = item
    End Sub

    Protected ReadOnly Property Item As IItem
End Class
