Imports FHOS.Persistence

Friend Class BaseAvatarModel
    Protected ReadOnly actor As IActor
    Protected Sub New(actor As IActor)
        Me.actor = actor
    End Sub
End Class
