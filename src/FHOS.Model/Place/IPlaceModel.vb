Public Interface IPlaceModel
    ReadOnly Property Name As String
    ReadOnly Property CanRefillOxygen As Boolean
    ReadOnly Property StarType As String
    ReadOnly Property PlanetVicinityCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property PlanetType As String
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property Parent As IPlaceModel
End Interface
