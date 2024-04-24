Imports FHOS.Persistence

Friend Class BaseAvatarModel
    Protected ReadOnly avatar As IActor
    Protected Sub DoTurn()
        avatar.Oxygen -= 1
    End Sub
    Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub
    Protected Sub SetLocation(location As ILocation)
        Dim isDifferentMap = location.Map.Id <> avatar.Location.Map.Id
        If isDifferentMap Then
            'TODO: leave old map
        End If
        Me.avatar.Location = location
        If isDifferentMap Then
            'TODO: enter new map
        End If
    End Sub
End Class
