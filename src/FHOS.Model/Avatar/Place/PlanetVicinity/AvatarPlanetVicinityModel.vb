Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarPlanetVicinityModel
    Inherits AvatarPlaceModel
    Implements IAvatarPlanetVicinityModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IPlanetVicinityModel Implements IAvatarPlanetVicinityModel.LegacyCurrent
        Get
            Dim planet = avatar.Location.Place
            If planet IsNot Nothing Then
                Return New PlanetVicinityModel(planet)
            End If
            Return Nothing
        End Get
    End Property
End Class
