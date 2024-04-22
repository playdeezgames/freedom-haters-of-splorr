Imports System.Security.Cryptography.X509Certificates
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
        Dim mapId As Integer
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
        If UniverseData.Maps.Recycled.Any Then
            mapId = UniverseData.Maps.Recycled.First
            UniverseData.Maps.Recycled.Remove(mapId)
            UniverseData.Maps.Entities(mapId) = mapData
        Else
            mapId = UniverseData.Maps.Entities.Count
            UniverseData.Maps.Entities.Add(mapData)
        End If
        mapData.Locations = Enumerable.
                    Range(0, columns * rows).
                    Select(Function(x) CreateLocation(locationType, mapId, x Mod rows, x \ rows).Id).ToList
        Return New Map(UniverseData, mapId)
    End Function

    Public Function CreateActor(actorType As String, location As ILocation) As IActor Implements IUniverse.CreateActor
        Dim actorData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.LocationId, location.Id},
                                        {StatisticTypes.Facing, 1}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.ActorType, actorType}
                                    }
                                 }
        Dim actorId As Integer
        If UniverseData.Actors.Recycled.Any Then
            actorId = UniverseData.Actors.Recycled.First
            UniverseData.Actors.Recycled.Remove(actorId)
            UniverseData.Actors.Entities(actorId) = actorData
        Else
            actorId = UniverseData.Actors.Entities.Count
            UniverseData.Actors.Entities.Add(actorData)
        End If
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
        Dim locationId As Integer
        If UniverseData.Locations.Recycled.Any Then
            locationId = UniverseData.Locations.Recycled.First
            UniverseData.Locations.Recycled.Remove(locationId)
            UniverseData.Locations.Entities(locationId) = locationData
        Else
            locationId = UniverseData.Locations.Entities.Count
            UniverseData.Locations.Entities.Add(locationData)
        End If
        Return New Location(UniverseData, locationId)
    End Function

    Public Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem Implements IUniverse.CreateStarSystem
        Dim starSystemId As Integer
        Dim starSystemData = New StarSystemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.StarType, starType}
                }
            }
        If UniverseData.StarSystems.Recycled.Any Then
            starSystemId = UniverseData.StarSystems.Recycled.First
            UniverseData.StarSystems.Recycled.Remove(starSystemId)
            UniverseData.StarSystems.Entities(starSystemId) = starSystemData
        Else
            starSystemId = UniverseData.StarSystems.Entities.Count
            UniverseData.StarSystems.Entities.Add(starSystemData)
        End If
        Return New StarSystem(UniverseData, starSystemId)
    End Function

    Public Function CreateTeleporter(target As ILocation) As ITeleporter Implements IUniverse.CreateTeleporter
        Dim teleporterId As Integer
        Dim teleporterData As New TeleporterData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.LocationId, target.Id}
                }
            }
        If UniverseData.Teleporters.Recycled.Any Then
            teleporterId = UniverseData.Teleporters.Recycled.First
            UniverseData.Teleporters.Recycled.Remove(teleporterId)
            UniverseData.Teleporters.Entities(teleporterId) = teleporterData
        Else
            teleporterId = UniverseData.Teleporters.Entities.Count
            UniverseData.Teleporters.Entities.Add(teleporterData)
        End If
        Return New Teleporter(UniverseData, teleporterId)
    End Function

    Public Function CreateStar(starName As String, starType As String) As IStar Implements IUniverse.CreateStar
        Dim starId As Integer
        Dim starData = New StarData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starName},
                    {MetadataTypes.StarType, starType}
                }
            }
        If UniverseData.Stars.Recycled.Any Then
            starId = UniverseData.Stars.Recycled.First
            UniverseData.Stars.Recycled.Remove(starId)
            UniverseData.Stars.Entities(starId) = starData
        Else
            starId = UniverseData.Stars.Entities.Count
            UniverseData.Stars.Entities.Add(starData)
        End If
        Return New Star(UniverseData, starId)
    End Function

    Public Function CreatePlanet(planetName As String, planetType As String) As IPlanet Implements IUniverse.CreatePlanet
        Dim planetId As Integer
        Dim planetData = New PlanetData With
            {
                .Metadatas = New Dictionary(Of String, String) From
{
                    {MetadataTypes.Name, planetName},
                    {MetadataTypes.PlanetType, planetType}
                }
            }
        If UniverseData.Planets.Recycled.Any Then
            planetId = UniverseData.Planets.Recycled.First
            UniverseData.Planets.Recycled.Remove(planetId)
            UniverseData.Planets.Entities(planetId) = planetData
        Else
            planetId = UniverseData.Planets.Entities.Count
            UniverseData.Planets.Entities.Add(planetData)
        End If
        Return New Planet(UniverseData, planetId)
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite Implements IUniverse.CreateSatellite
        Dim satelliteId As Integer
        Dim satelliteData = New SatelliteData With
            {
                .Metadatas = New Dictionary(Of String, String) From
{
                    {MetadataTypes.Name, satelliteName},
                    {MetadataTypes.SatelliteType, satelliteType}
                }
            }
        If UniverseData.Satellites.Recycled.Any Then
            satelliteId = UniverseData.Satellites.Recycled.First
            UniverseData.Satellites.Recycled.Remove(satelliteId)
            UniverseData.Satellites.Entities(satelliteId) = satelliteData
        Else
            satelliteId = UniverseData.Satellites.Entities.Count
            UniverseData.Satellites.Entities.Add(satelliteData)
        End If
        Return New Satellite(UniverseData, satelliteId)
    End Function
End Class
