Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class ItemDataClient
    Inherits TypedEntityDataClient(Of IItemData)

    Public Sub New(
                  universeData As IUniverseData,
                  entityId As Integer)
        MyBase.New(
            universeData,
            entityId,
            Function(u, i) u.Items(i),
            Sub(u, i) Return)
    End Sub
End Class
