Public Interface IUniverse
    Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, locationType As String) As IMap
    Function CreateActor(actorType As String, location As ILocation) As IActor
    Function CreateLocation(locationType As String, mapId As Integer, column As Integer, row As Integer) As ILocation
    Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem
    Function CreateTeleporter(target As ILocation) As ITeleporter
    Function CreateStar(starName As String, starType As String) As IStar
    Function CreatePlanet(planetName As String, planetType As String) As IPlanet
    Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite
    Property Avatar As IActor
End Interface
