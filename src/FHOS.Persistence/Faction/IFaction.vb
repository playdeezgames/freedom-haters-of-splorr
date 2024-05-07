Public Interface IFaction
    ReadOnly Property Id As Integer
    ReadOnly Property MinimumPlanetCount As Integer
    Function HasFlag(flag As String) As Boolean
End Interface
