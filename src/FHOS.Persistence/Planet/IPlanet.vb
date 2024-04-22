Public Interface IPlanet
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    ReadOnly Property PlanetType As String
    Property Map As IMap
End Interface
