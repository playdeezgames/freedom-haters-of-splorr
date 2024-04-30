Public Interface IPlanetVicinity
    Inherits IPlace
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    ReadOnly Property PlanetType As String
    Property Map As IMap
    Function CreatePlanet() As IPlanet
    Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace
    ReadOnly Property Identifier As String
End Interface
