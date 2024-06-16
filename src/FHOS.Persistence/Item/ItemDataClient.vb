Imports FHOS.Data

Friend Class ItemDataClient
    Inherits TypedEntityDataClient(Of ItemData)

    Public Sub New(
                  universeData As UniverseData,
                  entityId As Integer)
        MyBase.New(
            universeData,
            entityId,
            Function(u, i) u.Items.Entities(i),
            Sub(u, i) u.Items.Recycled.Add(i))
    End Sub
End Class
