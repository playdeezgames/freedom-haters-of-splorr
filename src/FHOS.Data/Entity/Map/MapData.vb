Public Class MapData
    Inherits EntityData
    Implements IMapData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Property YokedGroups As New Dictionary(Of String, Integer) Implements IMapData.YokedGroups

    Property Locations As New Dictionary(Of Integer, Integer)

    Public ReadOnly Property AllLocations As IEnumerable(Of Integer) Implements IMapData.AllLocations
        Get
            Return Locations.Values
        End Get
    End Property

    Public Sub SetLocation(index As Integer, locationId As Integer) Implements IMapData.SetLocation
        Locations(index) = locationId
    End Sub

    Public Function GetLocation(index As Integer) As Integer Implements IMapData.GetLocation
        Return Locations(index)
    End Function
End Class
