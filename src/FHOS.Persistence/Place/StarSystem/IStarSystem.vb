Public Interface IStarSystem
    Inherits IPlace
    ReadOnly Property Name As String
    ReadOnly Property Identifier As String
    Property Map As IMap
    ReadOnly Property StarType As String
    Function CreateStarVicinity() As IStarVicinity
    Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlanetVicinity
End Interface
