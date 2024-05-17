Public Interface IPlaceFactory
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
