Public Interface IGroupModel
    ReadOnly Property Name As String
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property PlanetList As IEnumerable(Of IGroupModel)
    ReadOnly Property SatelliteList As IEnumerable(Of IGroupModel)
    Function RelationNameTo(otherGroup As IGroupModel) As String
    ReadOnly Property StarSystem As IGroupModel
    ReadOnly Property Planet As IGroupModel
    ReadOnly Property Faction As IGroupModel
    ReadOnly Property StarTypeName As String
    ReadOnly Property Position As (Column As Integer, Row As Integer)
    ReadOnly Property FactionsPresent As IEnumerable(Of IGroupModel)
End Interface
