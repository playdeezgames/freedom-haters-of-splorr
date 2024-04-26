Public Interface IUniverse
    Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, locationType As String) As IMap
    Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem
    Property Avatar As IActor
    ReadOnly Property Messages As IMessages

    'TODO: place under location
    Function CreateActor(actorType As String, location As ILocation) As IActor
    Function CreateTeleporter(target As ILocation) As ITeleporter

    'TODO: place under map
    Function CreateLocation(locationType As String, mapId As Integer, column As Integer, row As Integer) As ILocation

End Interface
