Public Interface IFaction
    Inherits IEntity
    ReadOnly Property MinimumPlanetCount As Integer
    Function HasFlag(flag As String) As Boolean
End Interface
