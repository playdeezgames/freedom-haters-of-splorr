Public Interface IPlace
    ReadOnly Property Id As Integer
    ReadOnly Property Universe As IUniverse
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    ReadOnly Property PlaceType As String
    ReadOnly Property Parent As IPlace
    ReadOnly Property StarType As String
    Function CreateStarVicinity() As IStarVicinity
    Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlanetVicinity
    Function CreateStar() As IStar
    Function CreatePlanet() As IPlanet
    Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite
    ReadOnly Property PlanetType As String
End Interface
