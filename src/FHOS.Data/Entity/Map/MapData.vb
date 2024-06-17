Public Class MapData
    Inherits EntityData
    Implements IMapData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Property Locations As New List(Of Integer) Implements IMapData.Locations
    Property YokedGroups As New Dictionary(Of String, Integer) Implements IMapData.YokedGroups
End Class
