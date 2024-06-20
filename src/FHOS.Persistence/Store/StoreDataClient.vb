Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class StoreDataClient
    Inherits EntityDataClient(Of IStoreData)
    Sub New(
           universeData As IUniverseData,
           connection As SqliteConnection,
           actorId As Integer)
        MyBase.New(
            universeData,
            connection,
            actorId,
            Function(u, i) u.Stores(i),
            Sub(u, i) Return)
    End Sub
End Class
