Friend Class StarSystem
    Inherits StarSystemDataClient
    Implements IStarSystem

    Public Sub New(worldData As Data.WorldData, starSystemId As Integer)
        MyBase.New(worldData, starSystemId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStarSystem.Id
        Get
            Return StarSystemId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IStarSystem.Name
        Get
            Return StarSystemData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public Property Map As IMap Implements IStarSystem.Map
        Get
            Dim mapId As Integer
            If StarSystemData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(WorldData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                StarSystemData.Statistics.Remove(StatisticTypes.MapId)
            Else
                StarSystemData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements IStarSystem.World
        Get
            Return New World(WorldData)
        End Get
    End Property
End Class
