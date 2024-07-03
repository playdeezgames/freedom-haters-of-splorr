Public Class MapData
    Inherits GroupedEntityData
    Public Sub New(
                  id As Integer,
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(id, statistics:=statistics, flags:=flags, metadatas:=metadatas)
    End Sub

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
