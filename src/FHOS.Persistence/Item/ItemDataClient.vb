Imports FHOS.Data

Friend Class ItemDataClient
    Inherits TypedEntityDataClient(Of ItemData)

    Public Sub New(
                  universeData As UniverseData,
                  entityId As Integer)
        MyBase.New(
            universeData,
            entityId,
            Function(u, i) u.Items(i),
            Sub(u, i) u.Items.Remove(i))
    End Sub
End Class
