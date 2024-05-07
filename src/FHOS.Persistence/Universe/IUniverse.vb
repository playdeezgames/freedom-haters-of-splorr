Public Interface IUniverse
    Function CreateMap(
                      mapType As String,
                      mapName As String,
                      columns As Integer,
                      rows As Integer,
                      locationType As String) As IMap
    Function CreateStarSystem(
                             starSystemName As String,
                             starType As String) As IPlace
    Function CreateWormhole(wormholeName As String) As IPlace
    Property Avatar As IActor
    ReadOnly Property Messages As IMessages
    ReadOnly Property Places As IEnumerable(Of IPlace)
    Function CreateFaction(factionName As String, minimumPlanetCount As Integer, flags As IEnumerable(Of String)) As IFaction
    ReadOnly Property Factions As IEnumerable(Of IFaction)
End Interface
