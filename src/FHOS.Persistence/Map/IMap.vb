Public Interface IMap
    Inherits IEntity
    Function GetLocation(column As Integer, row As Integer) As ILocation
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    ReadOnly Property Universe As IUniverse
    Property Name As String
    ReadOnly Property Id As Integer
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    Function CreateLocation(locationType As String, column As Integer, row As Integer) As ILocation
End Interface
