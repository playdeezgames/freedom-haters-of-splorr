﻿Public Interface IPlanetVicinity
    Inherits IPlanet
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    Property Map As IMap
    ReadOnly Property Identifier As String

    Function CreatePlanet() As IPlanet
    Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace
End Interface