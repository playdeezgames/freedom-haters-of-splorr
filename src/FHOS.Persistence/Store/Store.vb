Imports FHOS.Data

Friend Class Store
    Inherits StoreDataClient
    Implements IStore

    Protected Sub New(universeData As UniverseData, storeId As Integer)
        MyBase.New(universeData, storeId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, storeId As Integer?) As IStore
        If storeId.HasValue Then
            Return New Store(universeData, storeId.Value)
        End If
        Return Nothing
    End Function
End Class
