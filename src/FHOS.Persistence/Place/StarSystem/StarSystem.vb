Imports FHOS.Data

Friend Class StarSystem
    Inherits PlaceDataClient
    Implements IStarSystem

    Public Sub New(universeData As Data.UniverseData, starSystemId As Integer)
        MyBase.New(universeData, starSystemId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStarSystem.Id
        Get
            Return PlaceId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IStarSystem.Name
        Get
            Return PlaceData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public Property Map As IMap Implements IStarSystem.Map
        Get
            Dim mapId As Integer
            If PlaceData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                PlaceData.Statistics.Remove(StatisticTypes.MapId)
            Else
                PlaceData.Statistics(StatisticTypes.MapId) = value.Id
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
            Return PlaceData.Metadatas(MetadataTypes.StarType)
        End Get
    End Property

    Public ReadOnly Property Identifier As String Implements IStarSystem.Identifier
        Get
            Return PlaceData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property

    Private Sub AddStarVicinity(starVicinity As IStarVicinity)
        PlaceData.Descendants.Add(starVicinity.Id)
    End Sub

    Private Sub AddPlanetVicinity(planetVicinity As IPlanetVicinity)
        PlaceData.Descendants.Add(planetVicinity.Id)
    End Sub

    Public Function CreateStarVicinity() As IStarVicinity Implements IStarSystem.CreateStarVicinity
        Dim starVicinity = New StarVicinity(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.StarType, StarType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.StarVicinity}
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
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetName},
                        {MetadataTypes.PlanetType, planetType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.PlanetVicinity}
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
