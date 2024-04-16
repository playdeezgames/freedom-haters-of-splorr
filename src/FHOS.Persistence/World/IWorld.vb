Public Interface IWorld
    Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, terrainType As String) As IMap
    Function CreateCharacter(characterType As String, cell As ICell) As ICharacter
    Function CreateCell(terrainType As String, mapId As Integer, column As Integer, row As Integer) As ICell
    Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem
    Function CreateTeleporter(destinationMap As IMap, flag As String) As ITeleporter
    Property Avatar As ICharacter
End Interface
