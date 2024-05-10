Public Interface IFaction
    Inherits IEntity
    ReadOnly Property MinimumPlanetCount As Integer
    ReadOnly Property Name As String
    Property PlanetCount As Integer
End Interface
