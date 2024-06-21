Imports FHOS.Data

Friend Class NamedEntityDataClient(Of TEntityData As IEntityData)
    Inherits TypedEntityDataClient(Of TEntityData)
    Implements INamedEntity
    Public Sub New(
              universeData As IUniverseData,
              entityId As Integer,
              entityDataFetcher As Func(Of IUniverseData, Integer, TEntityData),
              entityDataRecycler As Action(Of IUniverseData, Integer))
        MyBase.New(
            universeData,
            entityId,
            entityDataFetcher,
            entityDataRecycler)
    End Sub

    Public Property EntityName As String Implements INamedEntity.EntityName
        Get
            Return GetMetadata(LegacyMetadataTypes.Name)
        End Get
        Set(value As String)
            SetMetadata(LegacyMetadataTypes.Name, value)
        End Set
    End Property
End Class
