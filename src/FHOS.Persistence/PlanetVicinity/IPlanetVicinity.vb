Public Interface IPlanetVicinity
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    ReadOnly Property PlanetType As String
    Property Map As IMap
    Sub AddPlanet(planet As IPlanet)
    Sub AddSatellite(satellite As ISatellite)
End Interface
