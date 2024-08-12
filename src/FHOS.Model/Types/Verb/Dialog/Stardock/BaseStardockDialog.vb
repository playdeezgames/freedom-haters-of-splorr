Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class BaseStarDockDialog
    Inherits BaseDialog

    Protected Sub New(
                     dialogType As DialogType,
                     actor As IActor,
                     starDock As IActor,
                     finalDialog As IDialog)
        MyBase.New(dialogType, actor, finalDialog)
        Me.StarDock = starDock
    End Sub

    Protected ReadOnly Property StarDock As IActor
End Class
