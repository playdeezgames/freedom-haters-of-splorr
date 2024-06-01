Public Interface IMap
    Inherits IEntity
    ReadOnly Property EntityType As String
    Property EntityName As String
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    Function GetLocation(column As Integer, row As Integer) As ILocation
    ReadOnly Property Locations As IEnumerable(Of ILocation)
End Interface
