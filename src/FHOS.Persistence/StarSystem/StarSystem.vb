Imports FHOS.Data

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

    Private Sub AddStarVicinity(starVicinity As IStarVicinity)
        StarSystemData.StarVicinities.Add(starVicinity.Id)
    End Sub

    Private Sub AddPlanetVicinity(planetVicinity As IPlanetVicinity)
        StarSystemData.PlanetVicinities.Add(planetVicinity.Id)
    End Sub

    Public Function CreateStarVicinity() As IStarVicinity Implements IStarSystem.CreateStarVicinity
        Dim starVicinity = New StarVicinity(
            UniverseData,
                UniverseData.StarVicinities.CreateOrRecycle(
                New StarVicinityData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.StarType, StarType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarSystemId, Id}
                    }
                }))
        AddStarVicinity(starVicinity)
        Return starVicinity
    End Function

    Public Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlanetVicinity Implements IStarSystem.CreatePlanetVicinity
        Dim planetVicinity = New PlanetVicinity(
            UniverseData,
                UniverseData.PlanetVicinities.CreateOrRecycle(
                New PlanetVicinityData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetName},
                        {MetadataTypes.PlanetType, planetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarSystemId, Id}
                    }
                }))
        AddPlanetVicinity(planetVicinity)
        Return planetVicinity
    End Function
End Class
