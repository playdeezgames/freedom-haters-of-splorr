Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AcceptMissionDialog
    Implements IDialog

    Private ReadOnly actor As IActor
    Private ReadOnly starDock As IActor

    Public Sub New(actor As IActor, starDock As IActor)
        Me.actor = actor
        Me.starDock = starDock
    End Sub
End Class
