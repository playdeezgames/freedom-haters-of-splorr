Public Interface IFaction
    Inherits IEntity
    ReadOnly Property Id As Integer
    ReadOnly Property MinimumPlanetCount As Integer
    Function HasFlag(flag As String) As Boolean
End Interface
