Imports FHOS.Persistence

Friend Class BaseAvatarModel
    Protected ReadOnly avatar As IActor
    Protected Sub DoTurn()
        avatar.Oxygen -= 1
    End Sub
    Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub
End Class
