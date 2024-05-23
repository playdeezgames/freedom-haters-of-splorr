Public Interface IGroup
    Inherits IEntity
    ReadOnly Property MinimumPlanetCount As Integer
    ReadOnly Property Name As String
    Property PlanetCount As Integer
    ReadOnly Property Authority As Integer
    ReadOnly Property Standards As Integer
    ReadOnly Property Conviction As Integer
End Interface
