Public Interface IStarSystem
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    Property Map As IMap
    ReadOnly Property Universe As IUniverse
    ReadOnly Property StarType As String
    Function CreateStarVicinity() As IStarVicinity
    Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlanetVicinity
End Interface
