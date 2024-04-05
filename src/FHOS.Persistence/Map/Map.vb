Imports FHOS.Data

Friend Class Map
    Inherits MapDataClient
    Implements IMap

    Sub New(worldData As WorldData, mapId As Integer)
        MyBase.New(worldData, mapId)
    End Sub

    Public ReadOnly Property World As IWorld Implements IMap.World
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Function GetCell(column As Integer, row As Integer) As ICell Implements IMap.GetCell
        Throw New NotImplementedException()
    End Function
End Class
