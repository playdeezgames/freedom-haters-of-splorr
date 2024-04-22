Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarModel
    Implements IAvatarStarModel

    Private ReadOnly avatar As IActor

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

    Public ReadOnly Property CanApproach As Boolean Implements IAvatarStarModel.CanApproach
        Get
            Return Current IsNot Nothing
        End Get
    End Property

    Public Sub Approach() Implements IAvatarStarModel.Approach
        If CanApproach Then
            With avatar.Location.Star
                avatar.Location = RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Name)))
            End With
        End If
    End Sub
End Class
