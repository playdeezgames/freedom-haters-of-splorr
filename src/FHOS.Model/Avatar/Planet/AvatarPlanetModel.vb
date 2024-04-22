Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarPlanetModel
    Implements IAvatarPlanetModel

    Private ReadOnly avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Current As IPlanetModel Implements IAvatarPlanetModel.Current
        Get
            Dim planet = avatar.Location.Planet
            If planet IsNot Nothing Then
                Return New PlanetModel(planet)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property CanApproach As Boolean Implements IAvatarPlanetModel.CanApproach
        Get
            Return Current IsNot Nothing
        End Get
    End Property

    Public Sub Approach() Implements IAvatarPlanetModel.Approach
        If CanApproach Then
            With avatar.Location.Planet
                avatar.Location = RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Name)))
            End With
        End If
    End Sub
End Class
