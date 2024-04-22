Imports FHOS.Data

Friend Class Planet
    Inherits PlanetDataClient
    Implements IPlanet

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlanet.Id
        Get
            Return PlanetId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlanet.Name
        Get
            Return PlanetData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IPlanet.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Map As IMap Implements IPlanet.Map
        Get
            Dim mapId As Integer
            If PlanetData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                PlanetData.Statistics.Remove(StatisticTypes.MapId)
            Else
                PlanetData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property
End Class
