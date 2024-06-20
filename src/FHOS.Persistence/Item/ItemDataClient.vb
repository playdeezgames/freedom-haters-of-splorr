Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class ItemDataClient
    Inherits TypedEntityDataClient(Of IItemData)

    Public Sub New(
                  universeData As IUniverseData,
                  connection As SqliteConnection,
                  entityId As Integer)
        MyBase.New(
            universeData,
            connection,
            entityId,
            Function(u, i) u.Items(i),
            Sub(u, i) Return)
    End Sub
End Class
