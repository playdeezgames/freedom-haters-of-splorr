Imports FHOS.Persistence

Friend Class BaseAvatarModel
    Protected ReadOnly actor As IActor
    Protected Sub DoTurn()
        actor.Universe.Turn += 1
        actor.State.LifeSupport.CurrentValue -= 1
    End Sub
    Protected Sub New(actor As IActor)
        Me.actor = actor
    End Sub
    Protected Sub SetLocation(location As ILocation)
        Dim isDifferentMap = location.Map.Id <> actor.State.Location.Map.Id
        If isDifferentMap Then
            HandleMapExit(actor.State.Location.Map)
        End If
        Me.actor.State.Location = location
        If location.Place IsNot Nothing Then
            actor.KnownPlaces.Add(location.Place)
        End If
        If isDifferentMap Then
            HandleMapEntry(actor.State.Location.Map)
        End If
    End Sub

    Private Sub HandleMapEntry(map As IMap)
    End Sub

    Private Sub HandleMapExit(map As IMap)
    End Sub
End Class
