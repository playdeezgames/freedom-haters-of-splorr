Public Interface IPlace
    Inherits IEntity
    ReadOnly Property PlaceType As String
    ReadOnly Property Subtype As String

    'family
    Property PlanetCount As Integer
    Property SatelliteCount As Integer
    ReadOnly Property Parent As IPlace


    'properties
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    Property WormholeDestination As ILocation
    Property Faction As IFaction
    ReadOnly Property Position As (X As Integer, Y As Integer)

    'factory
    Function CreateStarVicinity(
                               x As Integer,
                               y As Integer) As IPlace
    Function CreatePlanetVicinity(
                                 planetName As String,
                                 planetType As String,
                                 x As Integer,
                                 y As Integer) As IPlace
    Function CreateStar() As IPlace
    Function CreatePlanet() As IPlace
    Function CreateSatellite(
                            satelliteName As String,
                            satelliteType As String) As IPlace
End Interface
