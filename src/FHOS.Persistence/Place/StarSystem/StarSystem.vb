Imports FHOS.Data

Friend Class StarSystem
    Inherits Place
    Implements IStarSystem

    Public Sub New(universeData As Data.UniverseData, starSystemId As Integer)
        MyBase.New(universeData, starSystemId)
    End Sub

    Public ReadOnly Property StarType As String Implements IStarSystem.StarType
        Get
            Return PlaceData.Metadatas(MetadataTypes.StarType)
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
