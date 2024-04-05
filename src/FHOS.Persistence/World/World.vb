Imports FHOS.Data

Public Class World
    Inherits WorldDataClient
    Implements IWorld
    Sub New(worldData As WorldData)
        MyBase.New(worldData)
    End Sub

    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.AvatarId = character.Id
    End Sub

    Public Function CreateMap(columns As Integer, rows As Integer, terrainType As String) As IMap Implements IWorld.CreateMap
        Dim mapId = WorldData.Maps.Count
        WorldData.Maps.Add(New MapData With
            {
                .Columns = columns,
                .Rows = rows,
                .Cells = Enumerable.
                    Range(0, columns * rows).
                    Select(Function(x) CreateCell(terrainType, mapId).Id).ToList
            })
        Return New Map(WorldData, mapId)
    End Function

    Public Function CreateCharacter(characterType As String, cell As ICell) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        WorldData.Characters.Add(New CharacterData With
                                 {
                                    .CharacterType = characterType,
                                    .CellId = cell.Id
                                 })
        Return New Character(WorldData, characterId)
    End Function

    Public Function CreateCell(terrainType As String, mapId As Integer) As ICell Implements IWorld.CreateCell
        Dim cellId = WorldData.Cells.Count
        WorldData.Cells.Add(New CellData With
                            {
                                .TerrainType = terrainType,
                                .MapId = mapId
                            })
        Return New Cell(WorldData, cellId)
    End Function
End Class
