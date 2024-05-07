Public Interface IPlace
    ReadOnly Property Id As Integer
    ReadOnly Property Universe As IUniverse
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    ReadOnly Property PlaceType As String
    ReadOnly Property Parent As IPlace
    ReadOnly Property StarType As String
    ReadOnly Property SatelliteType As String
    Function CreateStarVicinity() As IPlace
    Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlace
    Function CreateStar() As IPlace
    Function CreatePlanet() As IPlace
    Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace
    ReadOnly Property PlanetType As String
    Property WormholeDestination As ILocation
    Property Faction As IFaction
End Interface
