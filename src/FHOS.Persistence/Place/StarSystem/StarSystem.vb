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
        AddPlace(starVicinity)
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
        AddPlace(planetVicinity)
        Return planetVicinity
    End Function
End Class
