Public Interface IUniverseFactory
    Function CreateMap(
                      mapName As String,
                      mapType As String,
                      columns As Integer,
                      rows As Integer,
                      defaultLocationType As String) As IMap
    Function CreateStarSystem(
                             starSystemName As String,
                             starType As String,
                             x As Integer,
                             y As Integer) As IPlace
    Function CreateWormhole(wormholeName As String) As IPlace
    Function CreateFaction(
                          factionName As String,
                          minimumPlanetCount As Integer,
                          authority As Integer,
                          standards As Integer,
                          conviction As Integer) As IFaction
    Function CreateStore(
                        value As Integer,
                        Optional minimum As Integer? = Nothing,
                        Optional maximum As Integer? = Nothing) As IStore
End Interface
