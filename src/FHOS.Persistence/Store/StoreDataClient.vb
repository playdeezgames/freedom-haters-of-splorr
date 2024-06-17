Imports FHOS.Data

Friend Class StoreDataClient
    Inherits EntityDataClient(Of IStoreData)
    Sub New(
           universeData As IUniverseData,
           actorId As Integer)
        MyBase.New(
            universeData,
            actorId,
            Function(u, i) u.Stores.Entities(i),
            Sub(u, i) u.Stores.Recycled.Add(i))
    End Sub
End Class
