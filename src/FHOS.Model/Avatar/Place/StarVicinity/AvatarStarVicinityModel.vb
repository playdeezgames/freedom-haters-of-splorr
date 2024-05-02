Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarVicinityModel
    Inherits AvatarPlaceModel
    Implements IAvatarStarVicinityModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IStarVicinityModel Implements IAvatarStarVicinityModel.LegacyCurrent
        Get
            Dim star = avatar.Location.StarVicinity
            If star IsNot Nothing Then
                Return New StarVicinityModel(star)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property CanApproach As Boolean Implements IAvatarStarVicinityModel.CanApproach
        Get
            Return LegacyCurrent IsNot Nothing
        End Get
    End Property

    Public Sub Approach() Implements IAvatarStarVicinityModel.Approach
        If CanApproach Then
            DoTurn()
            With avatar.Location.StarVicinity
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub
End Class
