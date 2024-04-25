Imports System.Transactions
Imports FHOS.Data

Public Class Universe
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(universeData As UniverseData)
        MyBase.New(universeData)
    End Sub

    Public Property Avatar As IActor Implements IUniverse.Avatar
        Get
            Dim avatarId As Integer
            If UniverseData.Statistics.TryGetValue(StatisticTypes.AvatarId, avatarId) Then
                Return New Actor(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value IsNot Nothing Then
                UniverseData.Statistics(StatisticTypes.AvatarId) = value.Id
            Else
                UniverseData.Statistics.Remove(StatisticTypes.AvatarId)
            End If
        End Set
    End Property

    Public Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, locationType As String) As IMap Implements IUniverse.CreateMap
        Dim mapData = New MapData With
            {
                .Locations = Nothing,
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.Columns, columns},
                    {StatisticTypes.Rows, rows}
                },
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.MapType, mapType},
                    {MetadataTypes.Name, mapName}
                }
            }
        Dim mapId = CreateOrRecycle(UniverseData.Maps, mapData)
        mapData.Locations = Enumerable.
                            Range(0, columns * rows).
                            Select(Function(x) CreateLocation(locationType, mapId, x Mod rows, x \ rows).Id).ToList
        Return New Map(UniverseData, mapId)
    End Function

    Private Function CreateOrRecycle(Of TData)(bucket As BucketData(Of TData), data As TData) As Integer
        Dim entityId As Integer
        If bucket.Recycled.Any Then
            entityId = bucket.Recycled.First
            bucket.Recycled.Remove(entityId)
            bucket.Entities(entityId) = data
        Else
            entityId = bucket.Entities.Count
            bucket.Entities.Add(data)
        End If
        Return entityId
    End Function

    Public Function CreateActor(actorType As String, location As ILocation) As IActor Implements IUniverse.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.LocationId, location.Id},
                                        {StatisticTypes.Facing, 1},
                                        {StatisticTypes.Turn, 1}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.ActorType, actorType}
                                    }
                                 }
        Dim actorId As Integer = CreateOrRecycle(UniverseData.Actors, actorData)
        Dim actor = New Actor(UniverseData, actorId)
        location.Actor = actor
        Return actor
    End Function

    Public Function CreateLocation(locationType As String, mapId As Integer, column As Integer, row As Integer) As ILocation Implements IUniverse.CreateLocation
        Dim locationData = New LocationData With
                            {
                                .Statistics = New Dictionary(Of String, Integer) From
                                {
                                    {StatisticTypes.MapId, mapId},
                                    {StatisticTypes.Column, column},
                                    {StatisticTypes.Row, row}
                                },
                                .Metadatas = New Dictionary(Of String, String) From
                                {
                                    {MetadataTypes.LocationType, locationType}
                                }
                            }
        Dim locationId As Integer = CreateOrRecycle(UniverseData.Locations, locationData)
        Return New Location(UniverseData, locationId)
    End Function

    Public Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem Implements IUniverse.CreateStarSystem
        Dim starSystemData = New StarSystemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.StarType, starType}
                }
            }
        Dim starSystemId As Integer = CreateOrRecycle(UniverseData.StarSystems, starSystemData)
        Return New StarSystem(UniverseData, starSystemId)
    End Function

    Public Function CreateTeleporter(target As ILocation) As ITeleporter Implements IUniverse.CreateTeleporter
        Dim teleporterData As New TeleporterData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.LocationId, target.Id}
                }
            }
        Dim teleporterId As Integer = CreateOrRecycle(UniverseData.Teleporters, teleporterData)
        Return New Teleporter(UniverseData, teleporterId)
    End Function

    Public Function CreateStarVicinity(starSystem As IStarSystem) As IStarVicinity Implements IUniverse.CreateStarVicinity
        Dim starVicinity = New StarVicinity(
            UniverseData,
            CreateOrRecycle(
                UniverseData.StarVicinities,
                New StarVicinityData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, starSystem.Name},
                        {MetadataTypes.StarType, starSystem.StarType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarSystemId, starSystem.Id}
                    }
                }))
        starSystem.AddStarVicinity(starVicinity)
        Return starVicinity
    End Function

    Public Function CreatePlanetVicinity(starSystem As IStarSystem, planetName As String, planetType As String) As IPlanetVicinity Implements IUniverse.CreatePlanetVicinity
        Dim planetVicinity = New PlanetVicinity(
            UniverseData,
            CreateOrRecycle(
                UniverseData.PlanetVicinities,
                New PlanetVicinityData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetName},
                        {MetadataTypes.PlanetType, planetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarSystemId, starSystem.Id}
                    }
                }))
        starSystem.AddPlanetVicinity(planetVicinity)
        Return planetVicinity
    End Function

    Public Function CreateSatellite(planetVicinity As IPlanetVicinity, satelliteName As String, satelliteType As String) As ISatellite Implements IUniverse.CreateSatellite
        Dim satellite As ISatellite = New Satellite(
            UniverseData,
            CreateOrRecycle(
                UniverseData.Satellites,
                New SatelliteData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.SatelliteType, satelliteType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, planetVicinity.Id}
                    }
                }))
        planetVicinity.AddSatellite(satellite)
        Return satellite
    End Function

    Public Function CreatePlanet(planetVicinity As IPlanetVicinity) As IPlanet Implements IUniverse.CreatePlanet
        Dim planet As IPlanet = New Planet(
            UniverseData,
            CreateOrRecycle(
                UniverseData.Planets,
                New PlanetData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetVicinity.Name},
                        {MetadataTypes.PlanetType, planetVicinity.PlanetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, planetVicinity.Id}
                    }
                }))
        planetVicinity.AddPlanet(planet)
        Return planet
    End Function

    Public Function CreateStar(starVicinity As IStarVicinity) As IStar Implements IUniverse.CreateStar
        Dim star = New Star(
            UniverseData,
            CreateOrRecycle(
                UniverseData.Stars,
                New StarData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, starVicinity.Name},
                        {MetadataTypes.StarType, starVicinity.StarType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarVicinityId, starVicinity.Id}
                    }
                }))
        starVicinity.AddStar(star)
        Return star
    End Function
End Class
