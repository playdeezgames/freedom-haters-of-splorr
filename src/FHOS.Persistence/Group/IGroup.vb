Public Interface IGroup
    Inherits IEntity
    Property LegacyGroup As IGroup
    ReadOnly Property GroupType As String
    Property MinimumPlanetCount As Integer
    ReadOnly Property Name As String
    Property PlanetCount As Integer
    Property SatelliteCount As Integer
    Property Authority As Integer
    Property Standards As Integer
    Property Conviction As Integer
    ReadOnly Property Parents As IEnumerable(Of IGroup)
    Sub AddParent(parent As IGroup)
    Sub RemoveParent(parent As IGroup)
    ReadOnly Property Children As IEnumerable(Of IGroup)
End Interface
