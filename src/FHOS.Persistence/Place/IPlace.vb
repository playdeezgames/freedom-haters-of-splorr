Public Interface IPlace
    Inherits IEntity
    ReadOnly Property Universe As IUniverse
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    ReadOnly Property PlaceType As String
    ReadOnly Property Parent As IPlace
    ReadOnly Property StarType As String
    ReadOnly Property SatelliteType As String
    Function CreateStarVicinity() As IPlace
    Function CreatePlanetVicinity(planetName As String, planetType As String, x As Integer, y As Integer) As IPlace
    Function CreateStar() As IPlace
    Function CreatePlanet() As IPlace
    Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace
    ReadOnly Property PlanetType As String
    Property WormholeDestination As ILocation
    Property Faction As IFaction
    Property PlanetVicinityCount As Integer
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    Property SatelliteCount As Integer
End Interface
