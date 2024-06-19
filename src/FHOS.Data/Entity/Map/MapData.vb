Public Class MapData
    Inherits EntityData
    Implements IMapData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Property LegacyLocations As New List(Of Integer) Implements IMapData.LegacyLocations
    Property YokedGroups As New Dictionary(Of String, Integer) Implements IMapData.YokedGroups

    Public Property Locations As New Dictionary(Of Integer, Integer) Implements IMapData.Locations
End Class
