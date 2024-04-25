Public Interface IUniverse
    Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, locationType As String) As IMap
    Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem
    Property Avatar As IActor

    'TODO: place under location
    Function CreateActor(actorType As String, location As ILocation) As IActor
    Function CreateTeleporter(target As ILocation) As ITeleporter

    'TODO: place under map
    Function CreateLocation(locationType As String, mapId As Integer, column As Integer, row As Integer) As ILocation

    'TODO: put the following under the type of the first parameter
    Function CreateStarVicinity(starSystem As IStarSystem) As IStarVicinity
    Function CreatePlanetVicinity(starSystem As IStarSystem, planetName As String, planetType As String) As IPlanetVicinity
    Function CreatePlanet(planetVicinity As IPlanetVicinity) As IPlanet
    Function CreateSatellite(planetVicinity As IPlanetVicinity, satelliteName As String, satelliteType As String) As ISatellite
    Function CreateStar(starVicinity As IStarVicinity) As IStar
End Interface
