Public Interface IPlanetVicinity
    Inherits IPlanet

    Function CreatePlanet() As IPlanet
    Function LegacyCreateSatellite(satelliteName As String, satelliteType As String) As IPlace
    Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite
End Interface
