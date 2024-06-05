Imports FHOS.Data

Friend Class MapDataClient
    Inherits NamedEntityDataClient(Of MapData)
    Sub New(
           worldData As UniverseData,
           mapId As Integer)
        MyBase.New(
            worldData,
            mapId,
            Function(u, i) u.Maps.Entities(i),
            Sub(u, i) u.Maps.Recycled.Add(i))
    End Sub
End Class
