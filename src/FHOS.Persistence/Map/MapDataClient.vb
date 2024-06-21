Imports FHOS.Data

Friend Class MapDataClient
    Inherits NamedEntityDataClient(Of IMapData)
    Sub New(
           worldData As IUniverseData,
           mapId As Integer)
        MyBase.New(
            worldData,
            mapId,
            Function(u, i) u.Maps(i),
            Sub(u, i) Return)
    End Sub
End Class
