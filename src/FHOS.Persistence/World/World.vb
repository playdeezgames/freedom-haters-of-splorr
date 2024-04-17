Imports FHOS.Data

Public Class World
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(worldData As UniverseData)
        MyBase.New(worldData)
    End Sub

    Public Property Avatar As ICharacter Implements IUniverse.Avatar
        Get
            Dim avatarId As Integer
            If UniverseData.Statistics.TryGetValue(StatisticTypes.AvatarId, avatarId) Then
                Return New Character(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
        Set(value As ICharacter)
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
        If UniverseData.Actors.Recycled.Any Then
            characterId = UniverseData.Actors.Recycled.First
            UniverseData.Actors.Recycled.Remove(characterId)
            UniverseData.Actors.Entities(characterId) = characterData
        Else
            characterId = UniverseData.Actors.Entities.Count
            UniverseData.Actors.Entities.Add(characterData)
        End If
        Dim character = New Character(UniverseData, characterId)
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
        If UniverseData.RecycledLocations.Any Then
            cellId = UniverseData.RecycledLocations.First
            UniverseData.RecycledLocations.Remove(cellId)
            UniverseData.LegacyLocations(cellId) = cellData
        Else
            cellId = UniverseData.LegacyLocations.Count
            UniverseData.LegacyLocations.Add(cellData)
        End If
        Return New Cell(UniverseData, cellId)
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
        If UniverseData.RecycledStarSystems.Any Then
            starSystemId = UniverseData.RecycledStarSystems.First
            UniverseData.RecycledStarSystems.Remove(starSystemId)
            UniverseData.LegacyStarSystems(starSystemId) = starSystemData
        Else
            starSystemId = UniverseData.LegacyStarSystems.Count
            UniverseData.LegacyStarSystems.Add(starSystemData)
        End If
        Return New StarSystem(UniverseData, starSystemId)
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
        If UniverseData.RecycledTeleporters.Any Then
            teleporterId = UniverseData.RecycledTeleporters.First
            UniverseData.RecycledTeleporters.Remove(teleporterId)
            UniverseData.LegacyTeleporters(teleporterId) = teleporterData
        Else
            teleporterId = UniverseData.LegacyTeleporters.Count
            UniverseData.LegacyTeleporters.Add(teleporterData)
        End If
        Return New Teleporter(UniverseData, teleporterId)
    End Function
End Class
