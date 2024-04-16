Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarSystemModel
    Implements IAvatarStarSystemModel

    Private avatar As ICharacter

    Public Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Current As IStarSystemModel Implements IAvatarStarSystemModel.Current
        Get
            If avatar.Cell.StarSystem Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(avatar.Cell.StarSystem)
        End Get
    End Property

    Public ReadOnly Property CanEnter As Boolean Implements IAvatarStarSystemModel.CanEnter
        Get
            Return Current IsNot Nothing
        End Get
    End Property

    Public Sub Enter() Implements IAvatarStarSystemModel.Enter
        If CanEnter Then
            With avatar.Cell.StarSystem
                avatar.Cell = RNG.FromEnumerable(.Map.Cells.Where(Function(x) x.HasFlag(.Name)))
            End With
        End If
    End Sub
End Class
