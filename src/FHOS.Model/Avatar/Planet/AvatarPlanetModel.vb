Imports FHOS.Persistence

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
End Class
