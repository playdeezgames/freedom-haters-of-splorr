Imports FHOS.Data

Public Class World
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(worldData As UniverseData)
        MyBase.New(worldData)
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

    Public Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, terrainType As String) As IMap Implements IUniverse.CreateMap
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
                    Select(Function(x) CreateCell(terrainType, mapId, x Mod rows, x \ rows).Id).ToList
        Return New Map(UniverseData, mapId)
    End Function

    Public Function CreateCharacter(characterType As String, cell As ILocation) As IActor Implements IUniverse.CreateCharacter
        Dim characterData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.LocationId, cell.Id},
                                        {StatisticTypes.Facing, 1}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.ActorType, characterType}
                                    }
                                 }
        Dim characterId As Integer
        If UniverseData.Actors.Recycled.Any Then
            characterId = UniverseData.Actors.Recycled.First
            UniverseData.Actors.Recycled.Remove(characterId)
            UniverseData.Actors.Entities(characterId) = characterData
        Else
            characterId = UniverseData.Actors.Entities.Count
            UniverseData.Actors.Entities.Add(characterData)
        End If
        Dim character = New Actor(UniverseData, characterId)
        cell.Actor = character
        Return character
    End Function

    Public Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ILocation Implements IUniverse.CreateCell
        Dim cellData = New LocationData With
                            {
                                .Statistics = New Dictionary(Of String, Integer) From
                                {
                                    {StatisticTypes.MapId, mapId},
                                    {StatisticTypes.Column, column},
                                    {StatisticTypes.Row, row}
                                },
                                .Metadatas = New Dictionary(Of String, String) From
                                {
                                    {MetadataTypes.LocationType, terrainType}
                                }
                            }
        Dim cellId As Integer
        If UniverseData.Locations.Recycled.Any Then
            cellId = UniverseData.Locations.Recycled.First
            UniverseData.Locations.Recycled.Remove(cellId)
            UniverseData.Locations.Entities(cellId) = cellData
        Else
            cellId = UniverseData.Locations.Entities.Count
            UniverseData.Locations.Entities.Add(cellData)
        End If
        Return New Location(UniverseData, cellId)
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
End Class
