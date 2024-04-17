Public Interface IUniverse
    Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, terrainType As String) As IMap
    Function CreateActor(actorType As String, cell As ILocation) As IActor
    Function CreateLocation(locationType As String, mapId As Integer, column As Integer, row As Integer) As ILocation
    Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem
    Function CreateTeleporter(target As ILocation) As ITeleporter
    Property Avatar As IActor
End Interface
