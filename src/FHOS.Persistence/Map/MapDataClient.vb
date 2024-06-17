Imports FHOS.Data

Friend Class MapDataClient
    Inherits NamedEntityDataClient(Of IMapData)
    Sub New(
           worldData As IUniverseData,
           mapId As Integer)
        MyBase.New(
            worldData,
            mapId,
            Function(u, i) u.Maps.Entities(i),
            Sub(u, i) u.Maps.Recycled.Add(i))
    End Sub
End Class
