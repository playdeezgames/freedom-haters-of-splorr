Public Interface IWorld
    Function CreateMap(mapName As String, columns As Integer, rows As Integer, terrainType As String) As IMap
    Function CreateCharacter(characterType As String, cell As ICell) As ICharacter
    Sub SetAvatar(character As ICharacter)
    Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ICell
    ReadOnly Property Avatar As ICharacter
End Interface
