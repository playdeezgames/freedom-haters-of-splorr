Public Interface IMapData
    Inherits IEntityData
    Sub SetLocation(index As Integer, locationId As Integer)
    Function GetLocation(index As Integer) As Integer
    ReadOnly Property AllLocations As IEnumerable(Of Integer)
    Property YokedGroups As Dictionary(Of String, Integer)
    'Sub SetYokedGroup(yokeType As String, groupId As Integer?)
    'Function GetYokedGroup(yokeType As String) As Integer?
End Interface
