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


    Public Property EntityType As String Implements ITypedEntity.EntityType
        Get

            Return GetMetadata(LegacyMetadataTypes.EntityType)
        End Get
        Set(value As String)
            SetMetadata(LegacyMetadataTypes.EntityType, value)
        End Set
    End Property
End Class
