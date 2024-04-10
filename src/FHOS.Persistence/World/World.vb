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

    Public Function CreateMap(columns As Integer, rows As Integer, terrainType As String) As IMap Implements IWorld.CreateMap
        Dim mapId = WorldData.Maps.Count
        Dim mapData = New MapData With
            {
                .Columns = columns,
                .Rows = rows,
                .Cells = Enumerable.
                    Range(0, columns * rows).
                    Select(Function(x) CreateCell(terrainType, mapId, x Mod rows, x \ rows).Id).ToList
            }
        WorldData.Maps.Add(mapData)
        Return New Map(WorldData, mapId)
    End Function

    Public Function CreateCharacter(characterType As String, cell As ICell) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        Dim characterData = New CharacterData With
                                 {
                                    .CharacterType = characterType,
                                    .Facing = 0,
                                    .CellId = cell.Id
                                 }
        WorldData.Characters.Add(characterData)
        Dim character = New Character(WorldData, characterId)
        cell.Character = character
        Return character
    End Function

    Public Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ICell Implements IWorld.CreateCell
        Dim cellId = WorldData.Cells.Count
        Dim cellData = New CellData With
                            {
                                .TerrainType = terrainType,
                                .MapId = mapId,
                                .Column = column,
                                .Row = row,
                                .CharacterId = Nothing
                            }
        WorldData.Cells.Add(cellData)
        Return New Cell(WorldData, cellId)
    End Function
End Class
