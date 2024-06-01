Imports FHOS.Data

Friend Class EntityDataClient(Of TEntityData As EntityData)
    Inherits UniverseDataClient
    Implements IEntity
    Private ReadOnly entityId As Integer
    Private ReadOnly entityDataFetcher As Func(Of UniverseData, Integer, TEntityData)
    Private ReadOnly entityDataRecycler As Action(Of UniverseData, Integer)
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
            Return EntityData.Flags.Contains(flag)
        End Get
        Set(value As Boolean)
            If value Then
                EntityData.Flags.Add(flag)
            Else
                EntityData.Flags.Remove(flag)
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
            Dim result As Integer
            If EntityData.Statistics.TryGetValue(statisticType, result) Then
                Return result
            End If
            Return Nothing
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                EntityData.Statistics(statisticType) = value.Value
            Else
                EntityData.Statistics.Remove(statisticType)
            End If
        End Set
    End Property

    Public Sub New(
                  universeData As Data.UniverseData,
                  entityId As Integer,
                  entityDataFetcher As Func(Of UniverseData, Integer, TEntityData),
                  entityDataRecycler As Action(Of UniverseData, Integer))
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
        If value Is Nothing Then
            EntityData.Metadatas.Remove(metadataType)
        Else
            EntityData.Metadatas(metadataType) = value
        End If
    End Sub
    Protected Function GetMetadata(metadataType As String) As String
        Dim result As String = Nothing
        If EntityData.Metadatas.TryGetValue(metadataType, result) Then
            Return result
        End If
        Return Nothing
    End Function

    Public Overridable Sub Recycle() Implements IEntity.Recycle
        EntityData.Flags.Clear()
        EntityData.Statistics.Clear()
        EntityData.Metadatas.Clear()
        entityDataRecycler(UniverseData, Id)
    End Sub
End Class
