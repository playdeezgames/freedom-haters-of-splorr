Public Interface IUniverseFactory
    Function CreateMap(
                      mapName As String,
                      mapType As String,
                      columns As Integer,
                      rows As Integer,
                      defaultLocationType As String) As IMap
    Function CreateGroup(
                        groupType As String,
                        groupName As String) As IGroup
    Function CreateStore(
                        value As Integer,
                        Optional minimum As Integer? = Nothing,
                        Optional maximum As Integer? = Nothing) As IStore
    Function CreateItem(itemType As String) As IItem
End Interface
