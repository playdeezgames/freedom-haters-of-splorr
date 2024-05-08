Imports FHOS.Data

Friend Class EntityDataClient(Of TEntityData As EntityData)
    Inherits UniverseDataClient
    Implements IEntity
    Private ReadOnly entityId As Integer
    Private ReadOnly entityDataFetcher As Func(Of UniverseData, Integer, TEntityData)
    Protected ReadOnly Property EntityData As TEntityData
        Get
            Return entityDataFetcher(UniverseData, entityId)
        End Get
    End Property

    Public ReadOnly Property Id As Integer Implements IEntity.Id
        Get
            Return entityId
        End Get
    End Property

    Public Sub New(universeData As Data.UniverseData, entityId As Integer, entityDataFetcher As Func(Of UniverseData, Integer, TEntityData))
        MyBase.New(universeData)
        Me.entityId = entityId
        Me.entityDataFetcher = entityDataFetcher
    End Sub
End Class
