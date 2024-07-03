Imports FHOS.Data

Friend Class StoreDataClient
    Inherits EntityDataClient(Of StoreData)
    Sub New(
           universeData As UniverseData,
           actorId As Integer)
        MyBase.New(
            universeData,
            actorId,
            Function(u, i) u.Stores(i),
            Sub(u, i) Return)
    End Sub
End Class
