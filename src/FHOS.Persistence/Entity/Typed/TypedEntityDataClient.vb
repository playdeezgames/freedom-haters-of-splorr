Imports FHOS.Data

Friend Class TypedEntityDataClient(Of TEntityData As EntityData)
    Inherits EntityDataClient(Of TEntityData)

    Public Sub New(
                  universeData As UniverseData,
                  entityId As Integer,
                  entityDataFetcher As Func(Of UniverseData, Integer, TEntityData),
                  entityDataRecycler As Action(Of UniverseData, Integer))
        MyBase.New(
            universeData,
            entityId,
            entityDataFetcher,
            entityDataRecycler)
    End Sub
End Class
