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
        Dim mapId = UniverseData.NextMapId
        Dim mapData = New MapData(UniverseData.Connection, mapId) With
            {
                .Locations = Nothing
            }
        mapData.SetStatistic(PersistenceStatisticTypes.Columns, columns)
        mapData.SetStatistic(PersistenceStatisticTypes.Rows, rows)
        mapData.SetMetadata(LegacyMetadataTypes.EntityType, mapType)
        mapData.SetMetadata(LegacyMetadataTypes.Name, mapName)
        UniverseData.Maps.Add(mapId, mapData)
        Dim map = Persistence.Map.FromId(UniverseData, mapId)
        Dim indices = Enumerable.
                            Range(0, columns * rows)
        Dim LegacyLocations = indices.
                            Select(Function(x) CreateLocation(mapId, locationType, x Mod columns, x \ columns)).ToList
        mapData.Locations = indices.ToDictionary(Function(x) x, Function(x) LegacyLocations(x))
        Return map
    End Function


    Private Function CreateLocation(mapId As Integer, locationType As String, column As Integer, row As Integer) As Integer
        Dim locationId = UniverseData.NextLocationId
        Dim locationData = New LocationData(
                                UniverseData.Connection, locationId)
        locationData.SetStatistic(PersistenceStatisticTypes.MapId, mapId)
        locationData.SetStatistic(PersistenceStatisticTypes.Column, column)
        locationData.SetStatistic(PersistenceStatisticTypes.Row, row)
        locationData.SetMetadata(LegacyMetadataTypes.EntityType, locationType)
        UniverseData.Locations.Add(locationId, locationData)
        Return locationId
    End Function

    Public Function CreateGroup(
                                groupType As String,
                                groupName As String) As IGroup Implements IUniverseFactory.CreateGroup
        Dim groupId = UniverseData.NextGroupId
        Dim groupData = New GroupData(UniverseData.Connection, groupId)
        groupData.SetMetadata(LegacyMetadataTypes.EntityType, groupType)
        groupData.SetMetadata(LegacyMetadataTypes.Name, groupName)
        UniverseData.Groups.Add(groupId, groupData)
        Return Group.FromId(UniverseData, groupId)
    End Function

    Public Function CreateStore(
                               value As Integer,
                               Optional minimum As Integer? = Nothing,
                               Optional maximum As Integer? = Nothing) As IStore Implements IUniverseFactory.CreateStore
        If (minimum.HasValue AndAlso maximum.HasValue AndAlso maximum.Value < minimum.Value) Then
            Throw New ArgumentOutOfRangeException
        End If
        Dim storeId = UniverseData.NextStoreId
        Dim storeData = New StoreData(UniverseData.Connection, storeId)
        storeData.SetStatistic(PersistenceStatisticTypes.CurrentValue, value)
        storeData.SetStatistic(PersistenceStatisticTypes.MinimumValue, minimum)
        storeData.SetStatistic(PersistenceStatisticTypes.MaximumValue, maximum)
        UniverseData.Stores.Add(storeId, storeData)
        Return Store.FromId(UniverseData, storeId)
    End Function

    Public Function CreateItem(itemType As String) As IItem Implements IUniverseFactory.CreateItem
        Dim itemId = UniverseData.NextItemId
        Dim itemData = New ItemData(UniverseData.Connection, itemId)
        itemData.SetMetadata(LegacyMetadataTypes.EntityType, itemType)
        UniverseData.Items.Add(itemId, itemData)
        Return Item.FromId(UniverseData, itemId)
    End Function
End Class
