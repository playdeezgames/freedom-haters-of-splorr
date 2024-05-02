Friend Class PlanetVicinityModel
    Inherits PlaceModel
    Implements IPlanetVicinityModel

    Private ReadOnly planet As Persistence.IPlanetVicinity

    Public Sub New(planet As Persistence.IPlanetVicinity)
        MyBase.New(planet)
        Me.planet = planet
    End Sub
End Class
