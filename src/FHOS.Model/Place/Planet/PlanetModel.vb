Friend Class PlanetModel
    Inherits PlaceModel
    Implements IPlanetModel

    Private ReadOnly planet As Persistence.IPlace

    Public Sub New(planet As Persistence.IPlace)
        MyBase.New(planet)
        Me.planet = planet
    End Sub

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IPlanetModel.CanRefillOxygen
        Get
            Return PlanetTypes.Descriptors(planet.PlanetType).CanRefillOxygen
        End Get
    End Property
End Class
