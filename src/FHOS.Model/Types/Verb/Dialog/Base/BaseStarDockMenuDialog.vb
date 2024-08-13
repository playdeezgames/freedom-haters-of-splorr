Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseStarDockMenuDialog
    Inherits BaseMenuDialog

    Protected Sub New(
                     actor As IActor,
                     starDock As IActor,
                     finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
        Me.StarDock = starDock
    End Sub

    Protected ReadOnly Property StarDock As IActor
End Class
