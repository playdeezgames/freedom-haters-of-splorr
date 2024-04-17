Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarSystemModel
    Implements IAvatarStarSystemModel

    Private avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Current As IStarSystemModel Implements IAvatarStarSystemModel.Current
        Get
            If avatar.Location.StarSystem Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(avatar.Location.StarSystem)
        End Get
    End Property

    Public ReadOnly Property CanEnter As Boolean Implements IAvatarStarSystemModel.CanEnter
        Get
            Return Current IsNot Nothing
        End Get
    End Property

    Public Sub Enter() Implements IAvatarStarSystemModel.Enter
        If CanEnter Then
            With avatar.Location.StarSystem
                avatar.Location = RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Name)))
            End With
        End If
    End Sub
End Class
