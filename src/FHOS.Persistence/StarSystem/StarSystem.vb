Friend Class StarSystem
    Inherits StarSystemDataClient
    Implements IStarSystem

    Public Sub New(universeData As Data.UniverseData, starSystemId As Integer)
        MyBase.New(universeData, starSystemId)
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
                Return New Map(UniverseData, mapId)
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

    Public ReadOnly Property Universe As IUniverse Implements IStarSystem.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public ReadOnly Property StarType As String Implements IStarSystem.StarType
        Get
            Return StarSystemData.Metadatas(MetadataTypes.StarType)
        End Get
    End Property
End Class
