Public Interface IMap
    Inherits IEntity
    Function GetLocation(column As Integer, row As Integer) As ILocation
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    Property Name As String
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    Function CreateLocation(locationType As String, column As Integer, row As Integer) As ILocation
End Interface
