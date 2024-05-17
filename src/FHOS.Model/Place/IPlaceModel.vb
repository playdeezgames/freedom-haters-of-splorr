Public Interface IPlaceModel
    ReadOnly Property Name As String
    ReadOnly Property CanRefillOxygen As Boolean
    ReadOnly Property Subtype As String
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property Parent As IPlaceModel
End Interface
