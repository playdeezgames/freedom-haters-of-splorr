Public Interface IGroup
    Inherits IEntity
    Property Group As IGroup
    ReadOnly Property GroupType As String
    Property MinimumPlanetCount As Integer
    ReadOnly Property Name As String
    Property PlanetCount As Integer
    Property Authority As Integer
    Property Standards As Integer
    Property Conviction As Integer
End Interface
