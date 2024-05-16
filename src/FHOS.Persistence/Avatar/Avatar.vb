Friend Class Avatar
    Inherits UniverseDataClient
    Implements IAvatar

    Protected Sub New(universeData As Data.UniverseData)
        MyBase.New(universeData)
    End Sub

    Public ReadOnly Property Actor As IActor Implements IAvatar.Actor
        Get
            Dim avatarId As Integer
            If UniverseData.Avatars.TryPeek(avatarId) Then
                Return Persistence.Actor.FromId(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub Push(actor As IActor) Implements IAvatar.Push
        UniverseData.Avatars.Push(actor.Id)
    End Sub

    Friend Shared Function FromData(universeData As Data.UniverseData) As IAvatar
        Return New Avatar(universeData)
    End Function

    Public Function Pop() As IActor Implements IAvatar.Pop
        Return Persistence.Actor.FromId(UniverseData, UniverseData.Avatars.Pop())
    End Function
End Class
