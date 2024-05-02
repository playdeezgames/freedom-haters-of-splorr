Friend Class PlanetModel
    Inherits PlaceModel
    Implements IPlanetModel

    Private ReadOnly planet As Persistence.IPlanet

    Public Sub New(planet As Persistence.IPlanet)
        MyBase.New(planet)
        Me.planet = planet
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IPlanetModel.CanRefillOxygen
        Get
            Return PlanetTypes.Descriptors(planet.PlanetType).CanRefillOxygen
        End Get
    End Property
End Class
