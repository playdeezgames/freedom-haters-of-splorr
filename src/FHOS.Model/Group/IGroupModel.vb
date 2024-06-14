Public Interface IGroupModel
    ReadOnly Property Name As String
    Function RelationNameTo(otherGroup As IGroupModel) As String
    'parents
    ReadOnly Property ParentStarSystem As IGroupModel
    ReadOnly Property ParentPlanet As IGroupModel
    ReadOnly Property ParentFaction As IGroupModel
    'children
    ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel)
    'properties
    ReadOnly Property Properties As IGroupPropertiesModel
    ReadOnly Property StarTypeName As String
    ReadOnly Property Position As (Column As Integer, Row As Integer)
    ReadOnly Property PlanetCount As Integer
    ReadOnly Property SatelliteCount As Integer
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
End Interface
