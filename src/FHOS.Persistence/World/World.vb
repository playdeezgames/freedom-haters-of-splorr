Imports FHOS.Data

Public Class World
    Inherits WorldDataClient
    Implements IWorld
    Sub New(worldData As WorldData)
        MyBase.New(worldData)
    End Sub

    Public ReadOnly Property Avatar As ICharacter Implements IWorld.Avatar
        Get
            If WorldData.AvatarId.HasValue Then
                Return New Character(WorldData, WorldData.AvatarId.Value)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.AvatarId = character.Id
    End Sub

    Public Function CreateMap(mapName As String, columns As Integer, rows As Integer, terrainType As String) As IMap Implements IWorld.CreateMap
        Dim mapId As Integer
        Dim mapData = New MapData With
            {
                .Name = mapName,
                .Rows = rows,
                .Cells = Nothing,
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.Columns, columns}
                }
            }
        If WorldData.RecycledMaps.Any Then
            mapId = WorldData.RecycledMaps.First
            WorldData.RecycledMaps.Remove(mapId)
            WorldData.Maps(mapId) = mapData
        Else
            mapId = WorldData.Maps.Count
            WorldData.Maps.Add(mapData)
        End If
        mapData.Cells = Enumerable.
                    Range(0, columns * rows).
                    Select(Function(x) CreateCell(terrainType, mapId, x Mod rows, x \ rows).Id).ToList
        Return New Map(WorldData, mapId)
    End Function

    Public Function CreateCharacter(characterType As String, cell As ICell) As ICharacter Implements IWorld.CreateCharacter
        Dim characterData = New CharacterData With
                                 {
                                    .CharacterType = characterType,
                                    .Statistics = New Dictionary(Of String, Integer) From
                                    {
                                        {StatisticTypes.CellId, cell.Id}
                                    }
                                 }
        Dim characterId As Integer
        If WorldData.RecycledCharacters.Any Then
            characterId = WorldData.RecycledCharacters.First
            WorldData.RecycledCharacters.Remove(characterId)
            WorldData.Characters(characterId) = characterData
        Else
            characterId = WorldData.Characters.Count
            WorldData.Characters.Add(characterData)
        End If
        Dim character = New Character(WorldData, characterId)
        cell.Character = character
        Return character
    End Function

    Public Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ICell Implements IWorld.CreateCell
        Dim cellData = New CellData With
                            {
                                .TerrainType = terrainType,
                                .Statistics = New Dictionary(Of String, Integer) From
                                {
                                    {StatisticTypes.MapId, mapId},
                                    {StatisticTypes.Column, column},
                                    {StatisticTypes.Row, row}
                                }
                            }
        Dim cellId As Integer
        If WorldData.RecycledCells.Any Then
            cellId = WorldData.RecycledCells.First
            WorldData.RecycledCells.Remove(cellId)
            WorldData.Cells(cellId) = cellData
        Else
            cellId = WorldData.Cells.Count
            WorldData.Cells.Add(cellData)
        End If
        Return New Cell(WorldData, cellId)
    End Function

    Public Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem Implements IWorld.CreateStarSystem
        Dim starSystemId As Integer
        Dim starSystemData = New StarSystemData With
            {
                .Name = starSystemName,
                .StarType = starType
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
End Class
