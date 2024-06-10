Public Interface IMap
    Inherits INamedEntity
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
    Function GetLocation(column As Integer, row As Integer) As ILocation
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    ReadOnly Property YokedGroup(yokeType As String) As IGroup
End Interface
