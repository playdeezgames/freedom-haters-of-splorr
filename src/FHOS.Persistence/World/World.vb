Imports FHOS.Data

Public Class World
    Inherits WorldDataClient
    Implements IUniverse
    Sub New(worldData As UniverseData)
        MyBase.New(worldData)
    End Sub

    Public Property Avatar As ICharacter Implements IUniverse.Avatar
        Get
            Dim avatarId As Integer
            If WorldData.Statistics.TryGetValue(StatisticTypes.AvatarId, avatarId) Then
                Return New Character(WorldData, avatarId)
            End If
            Return Nothing
        End Get
        Set(value As ICharacter)
            If value IsNot Nothing Then
                WorldData.Statistics(StatisticTypes.AvatarId) = value.Id
            Else
                WorldData.Statistics.Remove(StatisticTypes.AvatarId)
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
        If WorldData.Maps.Recycled.Any Then
            mapId = WorldData.Maps.Recycled.First
            WorldData.Maps.Recycled.Remove(mapId)
            WorldData.Maps.Entities(mapId) = mapData
        Else
            mapId = WorldData.Maps.Entities.Count
            WorldData.Maps.Entities.Add(mapData)
        End If
        mapData.Locations = Enumerable.
                    Range(0, columns * rows).
                    Select(Function(x) CreateCell(terrainType, mapId, x Mod rows, x \ rows).Id).ToList
        Return New Map(WorldData, mapId)
    End Function

    Public Function CreateCharacter(characterType As String, cell As ICell) As ICharacter Implements IUniverse.CreateCharacter
        Dim characterData = New ActorData With
                                 {
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.CellId, cell.Id},
                                        {StatisticTypes.Facing, 1}
                                    },
                                    .Metadatas = New Dictionary(Of String, String) From
                                    {
                                        {MetadataTypes.CharacterType, characterType}
                                    }
                                 }
        Dim characterId As Integer
        If WorldData.RecycledActors.Any Then
            characterId = WorldData.RecycledActors.First
            WorldData.RecycledActors.Remove(characterId)
            WorldData.Actors(characterId) = characterData
        Else
            characterId = WorldData.Actors.Count
            WorldData.Actors.Add(characterData)
        End If
        Dim character = New Character(WorldData, characterId)
        cell.Character = character
        Return character
    End Function

    Public Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ICell Implements IUniverse.CreateCell
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
                                    {MetadataTypes.TerrainType, terrainType}
                                }
                            }
        Dim cellId As Integer
        If WorldData.RecycledLocations.Any Then
            cellId = WorldData.RecycledLocations.First
            WorldData.RecycledLocations.Remove(cellId)
            WorldData.Locations(cellId) = cellData
        Else
            cellId = WorldData.Locations.Count
            WorldData.Locations.Add(cellData)
        End If
        Return New Cell(WorldData, cellId)
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
        If WorldData.RecycledStarSystems.Any Then
            starSystemId = WorldData.RecycledStarSystems.First
            WorldData.RecycledStarSystems.Remove(starSystemId)
            WorldData.StarSystems(starSystemId) = starSystemData
        Else
            starSystemId = WorldData.StarSystems.Count
            WorldData.StarSystems.Add(starSystemData)
        End If
        Return New StarSystem(WorldData, starSystemId)
    End Function

    Public Function CreateTeleporter(target As ICell) As ITeleporter Implements IUniverse.CreateTeleporter
        Dim teleporterId As Integer
        Dim teleporterData As New TeleporterData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.CellId, target.Id}
                }
            }
        If WorldData.RecycledTeleporters.Any Then
            teleporterId = WorldData.RecycledTeleporters.First
            WorldData.RecycledTeleporters.Remove(teleporterId)
            WorldData.Teleporters(teleporterId) = teleporterData
        Else
            teleporterId = WorldData.Teleporters.Count
            WorldData.Teleporters.Add(teleporterData)
        End If
        Return New Teleporter(WorldData, teleporterId)
    End Function
End Class
