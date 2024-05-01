Public Interface IPlanetVicinity
    Inherits IPlanet

    Function CreatePlanet() As IPlanet
    Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace
End Interface
