﻿Imports FHOS.Data

Friend Class StoreDataClient
    Inherits EntityDataClient(Of IStoreData)
    Sub New(
           universeData As IUniverseData,
           actorId As Integer)
        MyBase.New(
            universeData,
            actorId,
            Function(u, i) u.Stores(i),
            Sub(u, i) Return)
    End Sub
End Class
