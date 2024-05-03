Imports FHOS.Data

Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlace.Id
        Get
            Return PlaceId
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IPlace.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property


    Public Property Name As String Implements IPlace.Name
        Get
            Return PlaceData.Metadatas(MetadataTypes.Name)
        End Get
        Set(value As String)
            PlaceData.Metadatas(MetadataTypes.Name) = value
        End Set
    End Property

    Public Property Map As IMap Implements IPlace.Map
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

    Public ReadOnly Property Identifier As String Implements IPlace.Identifier
        Get
            Return PlaceData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property

    Public ReadOnly Property PlaceType As String Implements IPlace.PlaceType
        Get
            Return PlaceData.Metadatas(MetadataTypes.PlaceType)
        End Get
    End Property

    Public ReadOnly Property Parent As IPlace Implements IPlace.Parent
        Get
            Dim parentId As Integer
            If PlaceData.Statistics.TryGetValue(StatisticTypes.ParentId, parentId) Then
                Return New Place(UniverseData, parentId)
            End If
            Return Nothing
        End Get
    End Property

    Protected Sub AddPlace(place As IPlace)
        PlaceData.Descendants.Add(place.Id)
    End Sub

    Public ReadOnly Property StarType As String Implements IPlace.StarType
        Get
            Return PlaceData.Metadatas(MetadataTypes.StarType)
        End Get
    End Property

    Public Function CreateStarVicinity() As IPlace Implements IPlace.CreateStarVicinity
        Dim starVicinity = New Place(
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
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(starVicinity)
        Return starVicinity
    End Function

    Public Function CreatePlanetVicinity(planetName As String, planetType As String) As IPlace Implements IPlace.CreatePlanetVicinity
        Dim planetVicinity = New Place(
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
                        {StatisticTypes.PlaceId, Id},
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(planetVicinity)
        Return planetVicinity
    End Function

    Public Function CreateStar() As IPlace Implements IPlace.CreateStar
        Dim star = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Star},
                        {MetadataTypes.StarType, StarType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(star)
        Return star
    End Function

    Public Function CreatePlanet() As IPlanet Implements IPlace.CreatePlanet
        Dim planet As IPlanet = New Planet(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Planet},
                        {MetadataTypes.PlanetType, PlanetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(planet)
        Return planet
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace Implements IPlace.CreateSatellite
        Dim satellite As IPlace = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.PlaceType, PlaceTypes.Satellite},
                        {MetadataTypes.SatelliteType, satelliteType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddSatellite(satellite)
        Return satellite
    End Function

    Private Sub AddSatellite(satellite As IPlace)
        PlaceData.Descendants.Add(satellite.Id)
    End Sub

    Public ReadOnly Property PlanetType As String Implements IPlace.PlanetType
        Get
            Return PlaceData.Metadatas(MetadataTypes.PlanetType)
        End Get
    End Property
End Class
