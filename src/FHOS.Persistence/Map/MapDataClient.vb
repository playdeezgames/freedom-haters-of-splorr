Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class MapDataClient
    Inherits NamedEntityDataClient(Of IMapData)
    Sub New(
           worldData As IUniverseData,
           connection As SqliteConnection,
           mapId As Integer)
        MyBase.New(
            worldData,
            connection,
            mapId,
            Function(u, i) u.Maps(i),
            Sub(u, i) Return)
    End Sub
End Class
