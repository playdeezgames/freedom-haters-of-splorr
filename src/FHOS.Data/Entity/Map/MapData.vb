Public Class MapData
    Inherits EntityData
    Implements IMapData
    Property Locations As New List(Of Integer) Implements IMapData.Locations
    Property YokedGroups As New Dictionary(Of String, Integer) Implements IMapData.YokedGroups
End Class
