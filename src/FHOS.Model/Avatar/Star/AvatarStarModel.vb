Imports FHOS.Persistence

Friend Class AvatarStarModel
    Implements IAvatarStarModel

    Private avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Current As IStarModel Implements IAvatarStarModel.Current
        Get
            Dim star = avatar.Location.Star
            If star IsNot Nothing Then
                Return New StarModel(star)
            End If
            Return Nothing
        End Get
    End Property
End Class
