Imports FHOS.Data

Friend Class PlanetVicinity
    Inherits PlanetVicinityDataClient
    Implements IPlanetVicinity

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlanetVicinity.Id
        Get
            Return PlanetVicinityId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlanetVicinity.Name
        Get
            Return PlanetVicinityData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IPlanetVicinity.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Map As IMap Implements IPlanetVicinity.Map
        Get
            Dim mapId As Integer
            If PlanetVicinityData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                PlanetVicinityData.Statistics.Remove(StatisticTypes.MapId)
            Else
                PlanetVicinityData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property

    Public ReadOnly Property PlanetType As String Implements IPlanetVicinity.PlanetType
        Get
            Return PlanetVicinityData.Metadatas(MetadataTypes.PlanetType)
        End Get
    End Property

    Public ReadOnly Property Identifier As String Implements IPlanetVicinity.Identifier
        Get
            Return PlanetVicinityData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property

    Private Sub AddPlanet(planet As IPlanet)
        PlanetVicinityData.Planets.Add(planet.Id)
    End Sub

    Private Sub AddSatellite(satellite As ISatellite)
        PlanetVicinityData.Satellites.Add(satellite.Id)
    End Sub

    Public Function CreatePlanet() As IPlanet Implements IPlanetVicinity.CreatePlanet
        Dim planet As IPlanet = New Planet(
            UniverseData,
                UniverseData.Planets.CreateOrRecycle(
                New PlanetData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlanetType, PlanetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, Id}
                    }
                }))
        AddPlanet(planet)
        Return planet
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite Implements IPlanetVicinity.CreateSatellite
        Dim satellite As ISatellite = New Satellite(
            UniverseData,
                UniverseData.Satellites.CreateOrRecycle(
                New SatelliteData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.SatelliteType, satelliteType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, Id}
                    }
                }))
        AddSatellite(satellite)
        Return satellite
    End Function
End Class
