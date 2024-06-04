Imports FHOS.Data

Friend Class NamedEntityDataClient(Of TEntityData As EntityData)
    Inherits TypedEntityDataClient(Of TEntityData)
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
