Imports FHOS.Data

Friend Class UniverseFactory
    Inherits UniverseDataClient
    Implements IUniverseFactory

    Public Sub New(universeData As Data.UniverseData)
        MyBase.New(universeData)
    End Sub

    Friend Shared Function FromData(universeData As Data.UniverseData) As IUniverseFactory
        Return New UniverseFactory(universeData)
    End Function

    Public Function CreateMap(
                             mapName As String,
                             mapType As String,
                             columns As Integer,
                             rows As Integer,
                             locationType As String) As IMap Implements IUniverseFactory.CreateMap
        Dim mapData = New MapData With
            {
                .Locations = Nothing,
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.Columns, columns},
                    {StatisticTypes.Rows, rows}
                },
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.MapType, mapType},
                    {MetadataTypes.Name, mapName}
                }
            }
        Dim mapId = UniverseData.Maps.CreateOrRecycle(mapData)
        Dim map = Persistence.Map.FromId(UniverseData, mapId)
        mapData.Locations = Enumerable.
                            Range(0, columns * rows).
                            Select(Function(x) map.CreateLocation(locationType, x Mod rows, x \ rows).Id).ToList
        Return map
    End Function

    Public Function CreateGroup(
                                 factionName As String,
                                 minimumPlanetCount As Integer,
                                 authority As Integer,
                                 standards As Integer,
                                 conviction As Integer) As IGroup Implements IUniverseFactory.CreateGroup
        Dim factionData = New GroupData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, factionName}
                },
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.MinimumPlanetCount, minimumPlanetCount},
                    {StatisticTypes.Authority, authority},
                    {StatisticTypes.Standards, standards},
                    {StatisticTypes.Conviction, conviction}
                }
            }
        Return Group.FromId(UniverseData, UniverseData.Groups.CreateOrRecycle(factionData))
    End Function

    Public Function CreateStore(
                               value As Integer,
                               Optional minimum As Integer? = Nothing,
                               Optional maximum As Integer? = Nothing) As IStore Implements IUniverseFactory.CreateStore
        Dim storeData = New StoreData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.CurrentValue, value}
                }
            }
        If minimum.HasValue Then
            storeData.Statistics(StatisticTypes.MinimumValue) = minimum.Value
        End If
        If maximum.HasValue Then
            storeData.Statistics(StatisticTypes.MaximumValue) = maximum.Value
        End If
        Return Store.FromId(UniverseData, UniverseData.Stores.CreateOrRecycle(storeData))
    End Function

    Public Function CreateItem(itemType As String) As IItem Implements IUniverseFactory.CreateItem
        Dim itemData = New ItemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.ItemType, itemType}
                }
            }
        Return Item.FromId(UniverseData, UniverseData.Items.CreateOrRecycle(itemData))
    End Function
End Class
