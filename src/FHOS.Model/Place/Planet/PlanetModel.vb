Friend Class PlanetModel
    Inherits PlaceModel
    Implements IPlanetModel

    Private ReadOnly planet As Persistence.IPlanet

    Public Sub New(planet As Persistence.IPlanet)
        Me.planet = planet
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IPlanetModel.CanRefillOxygen
        Get
            Return PlanetTypes.Descriptors(planet.PlanetType).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlanetModel.Name
        Get
            Return planet.Name
        End Get
    End Property
End Class
