Public Interface IMapData
    Inherits IGroupedEntityData
    Sub SetLocation(index As Integer, locationId As Integer)
    Function GetLocation(index As Integer) As Integer
    ReadOnly Property AllLocations As IEnumerable(Of Integer)
End Interface
