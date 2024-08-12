Public Interface IGroupPropertiesModel
    ReadOnly Property StarTypeName As String
    ReadOnly Property PlanetTypeName As String
    ReadOnly Property SatelliteTypeName As String
    ReadOnly Property Position As (Column As Integer, Row As Integer)
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
    ReadOnly Property TechLevel As Integer
    ReadOnly Property Values As IEnumerable(Of IGroupValueModel)
End Interface
