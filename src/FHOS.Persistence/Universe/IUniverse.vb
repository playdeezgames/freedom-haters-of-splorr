Public Interface IUniverse
    Function CreateMap(
                      mapType As String,
                      mapName As String,
                      columns As Integer,
                      rows As Integer,
                      locationType As String) As IMap
    Function CreateStarSystem(
                             starSystemName As String,
                             starType As String,
                             x As Integer,
                             y As Integer) As IPlace
    Function CreateWormhole(wormholeName As String) As IPlace
    Sub PushAvatar(avatar As IActor)
    Function PopAvatar() As IActor
    ReadOnly Property Avatar As IActor
    ReadOnly Property Messages As IMessages
    ReadOnly Property Places As IEnumerable(Of IPlace)
    Function GetPlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    Function CreateFaction(
                          factionName As String,
                          minimumPlanetCount As Integer,
                          flags As IEnumerable(Of String),
                          authority As Integer,
                          standards As Integer,
                          conviction As Integer) As IFaction
    Function CreateStore(value As Integer, Optional minimum As Integer? = Nothing, Optional maximum As Integer? = Nothing) As IStore
    ReadOnly Property Factions As IEnumerable(Of IFaction)
    ReadOnly Property Actors As IEnumerable(Of IActor)
End Interface
