Imports FHOS.Data

Friend Class UniverseFactory
    Inherits UniverseDataClient
    Implements IUniverseFactory

    Public Sub New(universeData As Data.IUniverseData)
        MyBase.New(universeData)
    End Sub

    Friend Shared Function FromData(universeData As Data.IUniverseData) As IUniverseFactory
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
                    {PersistenceStatisticTypes.Columns, columns},
                    {PersistenceStatisticTypes.Rows, rows}
                },
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, mapType},
                    {LegacyMetadataTypes.Name, mapName}
                }
            }
        Dim mapId = UniverseData.Maps.CreateOrRecycle(mapData)
        Dim map = Persistence.Map.FromId(UniverseData, mapId)
        mapData.Locations = Enumerable.
                            Range(0, columns * rows).
                            Select(Function(x) CreateLocation(mapId, locationType, x Mod columns, x \ columns)).ToList
        Return map
    End Function


    Private Function CreateLocation(mapId As Integer, locationType As String, column As Integer, row As Integer) As Integer
        Dim locationData = New LocationData With
                            {
                                .Statistics = New Dictionary(Of String, Integer) From
                                {
                                    {PersistenceStatisticTypes.MapId, mapId},
                                    {PersistenceStatisticTypes.Column, column},
                                    {PersistenceStatisticTypes.Row, row}
                                },
                                .Metadatas = New Dictionary(Of String, String) From
                                {
                                    {LegacyMetadataTypes.EntityType, locationType}
                                }
                            }
        Return UniverseData.Locations.CreateOrRecycle(locationData)
    End Function


    Public Function CreateGroup(
                                groupType As String,
                                groupName As String) As IGroup Implements IUniverseFactory.CreateGroup
        Dim factionData = New GroupData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, groupType},
                    {LegacyMetadataTypes.Name, groupName}
                }
            }
        Return Group.FromId(UniverseData, UniverseData.Groups.CreateOrRecycle(factionData))
    End Function

    Public Function CreateStore(
                               value As Integer,
                               Optional minimum As Integer? = Nothing,
                               Optional maximum As Integer? = Nothing) As IStore Implements IUniverseFactory.CreateStore
        If (minimum.HasValue AndAlso maximum.HasValue AndAlso maximum.Value < minimum.Value) Then
            Throw New ArgumentOutOfRangeException
        End If
        Dim storeData = New StoreData With
            {
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {PersistenceStatisticTypes.CurrentValue, value}
                }
            }
        If minimum.HasValue Then
            storeData.Statistics(PersistenceStatisticTypes.MinimumValue) = minimum.Value
        End If
        If maximum.HasValue Then
            storeData.Statistics(PersistenceStatisticTypes.MaximumValue) = maximum.Value
        End If
        Return Store.FromId(UniverseData, UniverseData.Stores.CreateOrRecycle(storeData))
    End Function

    Public Function CreateItem(itemType As String) As IItem Implements IUniverseFactory.CreateItem
        Dim itemData = New ItemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, itemType}
                }
            }
        Return Item.FromId(UniverseData, UniverseData.Items.CreateOrRecycle(itemData))
    End Function
End Class
