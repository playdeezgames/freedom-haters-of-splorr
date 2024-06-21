Imports FHOS.Data

Friend Class EntityDataClient(Of TEntityData As IEntityData)
    Inherits UniverseDataClient
    Implements IEntity
    Private ReadOnly entityId As Integer
    Private ReadOnly entityDataFetcher As Func(Of IUniverseData, Integer, TEntityData)
    Private ReadOnly entityDataRecycler As Action(Of IUniverseData, Integer)
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

    Public Property Flags(flag As String) As Boolean Implements IEntity.Flags
        Get
            Return EntityData.HasFlag(flag)
        End Get
        Set(value As Boolean)
            If value Then
                EntityData.SetFlag(flag)
            Else
                EntityData.ClearFlag(flag)
            End If
        End Set
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IEntity.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Statistics(statisticType As String) As Integer? Implements IEntity.Statistics
        Get
            Return EntityData.GetStatistic(statisticType)
        End Get
        Set(value As Integer?)
            EntityData.SetStatistic(statisticType, value)
        End Set
    End Property

    Public Property Metadatas(metadataType As String) As String Implements IEntity.Metadatas
        Get
            Return GetMetadata(metadataType)
        End Get
        Set(value As String)
            SetMetadata(metadataType, value)
        End Set
    End Property

    Public Sub New(
                  universeData As Data.IUniverseData,
                  entityId As Integer,
                  entityDataFetcher As Func(Of IUniverseData, Integer, TEntityData),
                  entityDataRecycler As Action(Of IUniverseData, Integer))
        MyBase.New(universeData)
        Me.entityId = entityId
        Me.entityDataFetcher = entityDataFetcher
        Me.entityDataRecycler = entityDataRecycler
    End Sub
    Protected Sub SetStatistic(statisticType As String, value As Integer?)
        Statistics(statisticType) = value
    End Sub
    Protected Function GetStatistic(statisticType As String) As Integer?
        Return Statistics(statisticType)
    End Function
    Protected Sub SetMetadata(metadataType As String, value As String)
        EntityData.SetMetadata(metadataType, value)
    End Sub
    Protected Function GetMetadata(metadataType As String) As String
        Return EntityData.GetMetadata(metadataType)
    End Function

    Public Overridable Sub Recycle() Implements IEntity.Recycle
        entityDataRecycler(UniverseData, Id)
    End Sub
End Class
