Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseItemMenuDialog
    Inherits BaseMenuDialog

    Protected ReadOnly item As IItem

    Protected Sub New(actor As Persistence.IActor, item As IItem, finalDialog As IDialog, prompt As String)
        MyBase.New(actor, finalDialog, prompt)
        Me.item = item
    End Sub
End Class
