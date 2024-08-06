Imports FHOS.Persistence

Friend MustInherit Class BaseStarDockDialog
    Inherits BaseDialog

    Protected Sub New(
                     actor As IActor,
                     starDock As IActor)
        MyBase.New(actor)
        Me.StarDock = starDock
    End Sub

    Protected ReadOnly Property StarDock As IActor
End Class
