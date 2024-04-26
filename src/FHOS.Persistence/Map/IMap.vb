Public Interface IMap
    Function GetLocation(column As Integer, row As Integer) As ILocation
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    ReadOnly Property Universe As IUniverse
    ReadOnly Property Name As String
    ReadOnly Property Id As Integer
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    Property StarSystem As IStarSystem
    Property StarVicinity As IStarVicinity
End Interface
