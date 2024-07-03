Imports FHOS.Data

Friend Class ItemDataClient
    Inherits TypedEntityDataClient(Of ItemData)

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
