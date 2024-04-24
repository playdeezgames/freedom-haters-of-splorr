Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarSystemModel
    Inherits BaseAvatarModel
    Implements IAvatarStarSystemModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
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
            DoTurn()
            With avatar.Location.StarSystem
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Name))))
            End With
        End If
    End Sub
End Class
