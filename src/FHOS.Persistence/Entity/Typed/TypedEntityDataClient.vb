Imports FHOS.Data

Friend Class TypedEntityDataClient(Of TEntityData As EntityData)
    Inherits EntityDataClient(Of TEntityData)
    Implements ITypedEntity

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


    Public ReadOnly Property EntityType As String Implements ITypedEntity.EntityType
        Get
            Return EntityData.Metadatas(LegacyMetadataTypes.EntityType)
        End Get
    End Property
End Class
