Friend Class Avatar
    Inherits UniverseDataClient
    Implements IAvatar

    Protected Sub New(universeData As Data.UniverseData)
        MyBase.New(universeData)
    End Sub

    Public ReadOnly Property AvatarActor As IActor Implements IAvatar.AvatarActor
        Get
            Dim avatarId As Integer
            If UniverseData.Avatars.TryPeek(avatarId) Then
                Return Actor.FromId(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub PushAvatar(avatar As IActor) Implements IAvatar.PushAvatar
        UniverseData.Avatars.Push(avatar.Id)
    End Sub

    Friend Shared Function FromData(universeData As Data.UniverseData) As IAvatar
        Return New Avatar(universeData)
    End Function

    Public Function PopAvatar() As IActor Implements IAvatar.PopAvatar
        Return Actor.FromId(UniverseData, UniverseData.Avatars.Pop())
    End Function
End Class
