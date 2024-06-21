Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class TypedEntityDataClient(Of TEntityData As IEntityData)
    Inherits EntityDataClient(Of TEntityData)
    Implements ITypedEntity

    Public Sub New(
                  universeData As IUniverseData, connection As SqliteConnection,
                  entityId As Integer,
                  entityDataFetcher As Func(Of IUniverseData, Integer, TEntityData),
                  entityDataRecycler As Action(Of IUniverseData, Integer))
        MyBase.New(
            universeData,
            connection,
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
