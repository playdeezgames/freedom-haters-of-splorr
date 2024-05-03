Imports FHOS.Persistence

Friend Class AvatarPlanetModel
    Inherits AvatarPlaceModel
    Implements IAvatarPlanetModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IAvatarPlanetModel.CanRefillOxygen
        Get
            Return If(LegacyCurrent?.CanRefillOxygen, False)
        End Get
    End Property

    Public ReadOnly Property LegacyCurrent As IPlanetModel Implements IAvatarPlanetModel.LegacyCurrent
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return New PlanetModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub RefillOxygen() Implements IAvatarPlanetModel.RefillOxygen
        If CanRefillOxygen Then
            avatar.Oxygen = avatar.MaximumOxygen
        End If
    End Sub
End Class
