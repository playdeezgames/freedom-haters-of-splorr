Public Interface IStarSystem
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    Property Map As IMap
    ReadOnly Property Universe As IUniverse
    ReadOnly Property StarType As String
    Sub AddStarVicinity(starVicinity As IStarVicinity)
    Sub AddPlanetVicinity(planetVicinity As IPlanetVicinity)
End Interface
