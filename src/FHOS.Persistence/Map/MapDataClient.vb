Imports FHOS.Data

Friend Class MapDataClient
    Inherits EntityDataClient(Of MapData)
    Sub New(worldData As UniverseData, mapId As Integer)
        MyBase.New(worldData, mapId, Function(u, i) u.Maps.Entities(i))
    End Sub
End Class
