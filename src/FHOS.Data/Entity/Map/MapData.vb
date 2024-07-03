Public Class MapData
    Inherits GroupedEntityData

    Property Locations As New Dictionary(Of Integer, Integer)

    Public ReadOnly Property AllLocations As IEnumerable(Of Integer)
        Get
            Return Locations.Values
        End Get
    End Property

    Public Sub SetLocation(index As Integer, locationId As Integer)
        Locations(index) = locationId
    End Sub

    Public Function GetLocation(index As Integer) As Integer
        Return Locations(index)
    End Function
End Class
