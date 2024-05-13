Public Interface IFactionModel
    ReadOnly Property Name As String
    ReadOnly Property Authority As Integer
    ReadOnly Property Standards As Integer
    ReadOnly Property Conviction As Integer
    ReadOnly Property PlanetCount As Integer
    Function RelationNameTo(otherFaction As IFactionModel) As String
End Interface
