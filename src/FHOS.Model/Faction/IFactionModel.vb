Public Interface IFactionModel
    ReadOnly Property Name As String
    ReadOnly Property Authority As (LevelName As String, Value As Integer)
    ReadOnly Property Standards As (LevelName As String, Value As Integer)
    ReadOnly Property Conviction As (LevelName As String, Value As Integer)
    ReadOnly Property PlanetCount As Integer
    Function RelationNameTo(otherFaction As IFactionModel) As String
End Interface
