Imports FHOS.Persistence

Friend Class AvatarPlanetModel
    Inherits AvatarPlaceModel
    Implements IAvatarPlanetModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IAvatarPlanetModel.CanRefillOxygen
        Get
            Return If(Current?.CanRefillOxygen, False)
        End Get
    End Property

    Public ReadOnly Property Current As IPlanetModel Implements IAvatarPlanetModel.Current
        Get
            If avatar.Location.Planet IsNot Nothing Then
                Return New PlanetModel(avatar.Location.Planet)
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
