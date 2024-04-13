Imports FHOS.Data

Friend Class Map
    Inherits MapDataClient
    Implements IMap

    Sub New(worldData As WorldData, mapId As Integer)
        MyBase.New(worldData, mapId)
    End Sub

    Public ReadOnly Property World As IWorld Implements IMap.World
        Get
            Return New World(WorldData)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IMap.Name
        Get
            Return MapData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Id As Integer Implements IMap.Id
        Get
            Return MapId
        End Get
    End Property

    Public Function GetCell(column As Integer, row As Integer) As ICell Implements IMap.GetCell
        If column < 0 OrElse row < 0 OrElse column >= MapData.Statistics(StatisticTypes.Columns) OrElse row >= MapData.Statistics(StatisticTypes.Rows) Then
            Return Nothing
        End If
        Return New Cell(WorldData, MapData.Cells(column + row * MapData.Statistics(StatisticTypes.Columns)))
    End Function
End Class
