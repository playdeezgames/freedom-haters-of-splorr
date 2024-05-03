Imports FHOS.Persistence

Friend Class AvatarPlanetModel
    Inherits AvatarPlaceModel
    Implements IAvatarPlanetModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IPlanetModel Implements IAvatarPlanetModel.LegacyCurrent
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return New PlanetModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property
End Class
