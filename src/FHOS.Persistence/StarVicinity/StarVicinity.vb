Friend Class StarVicinity
    Inherits StarVicinityDataClient
    Implements IStarVicinity

    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData, starId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStarVicinity.Id
        Get
            Return StarVicinityId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IStarVicinity.Name
        Get
            Return StarVicinityData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IStarVicinity.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Map As IMap Implements IStarVicinity.Map
        Get
            Dim mapId As Integer
            If StarVicinityData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                StarVicinityData.Statistics.Remove(StatisticTypes.MapId)
            Else
                StarVicinityData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property

    Public ReadOnly Property StarType As String Implements IStarVicinity.StarType
        Get
            Return StarVicinityData.Metadatas(MetadataTypes.StarType)
        End Get
    End Property
End Class
