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
            HandleMapExit(avatar.Location.Map)
        End If
        Me.avatar.Location = location
        If isDifferentMap Then
            HandleMapEntry(avatar.Location.Map)
        End If
    End Sub

    Private Sub HandleMapEntry(map As IMap)
        If map.StarSystem IsNot Nothing Then
            avatar.AddStarSystem(map.StarSystem)
        End If
    End Sub

    Private Sub HandleMapExit(map As IMap)
    End Sub
End Class
