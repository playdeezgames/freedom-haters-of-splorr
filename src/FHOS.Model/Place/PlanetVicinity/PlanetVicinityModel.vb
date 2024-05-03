Friend Class PlanetVicinityModel
    Inherits PlaceModel
    Implements IPlanetVicinityModel

    Private ReadOnly planet As Persistence.IPlace

    Public Sub New(planet As Persistence.IPlace)
        MyBase.New(planet)
        Me.planet = planet
    End Sub
End Class
