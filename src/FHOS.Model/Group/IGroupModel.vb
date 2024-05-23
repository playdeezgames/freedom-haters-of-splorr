Public Interface IGroupModel
    ReadOnly Property Name As String
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
    ReadOnly Property PlanetCount As Integer
    Function RelationNameTo(otherGroup As IGroupModel) As String
End Interface
