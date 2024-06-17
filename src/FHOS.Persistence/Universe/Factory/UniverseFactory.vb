Imports System.Diagnostics.CodeAnalysis
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
        Dim mapData = New MapData(New Dictionary(Of String, Integer) From
                {
                    {PersistenceStatisticTypes.Columns, columns},
                    {PersistenceStatisticTypes.Rows, rows}
                }) With
            {
                .Locations = Nothing,
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, mapType},
                    {LegacyMetadataTypes.Name, mapName}
                }
            }
        Dim mapId = UniverseData.Maps.Count
        UniverseData.Maps.Add(mapData)
        Dim map = Persistence.Map.FromId(UniverseData, mapId)
        mapData.Locations = Enumerable.
                            Range(0, columns * rows).
                            Select(Function(x) CreateLocation(mapId, locationType, x Mod columns, x \ columns)).ToList
        Return map
    End Function


    Private Function CreateLocation(mapId As Integer, locationType As String, column As Integer, row As Integer) As Integer
        Dim locationData = New LocationData(New Dictionary(Of String, Integer) From
                                {
                                    {PersistenceStatisticTypes.MapId, mapId},
                                    {PersistenceStatisticTypes.Column, column},
                                    {PersistenceStatisticTypes.Row, row}
                                }) With
                            {
                                .Metadatas = New Dictionary(Of String, String) From
                                {
                                    {LegacyMetadataTypes.EntityType, locationType}
                                }
                            }
        Dim locationId = UniverseData.Locations.Count
        UniverseData.Locations.Add(locationData)
        Return locationId
    End Function


    Public Function CreateGroup(
                                groupType As String,
                                groupName As String) As IGroup Implements IUniverseFactory.CreateGroup
        Dim groupData = New GroupData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, groupType},
                    {LegacyMetadataTypes.Name, groupName}
                }
            }
        Dim groupId = UniverseData.Groups.Count
        UniverseData.Groups.Add(groupData)
        Return Group.FromId(UniverseData, groupId)
    End Function

    Public Function CreateStore(
                               value As Integer,
                               Optional minimum As Integer? = Nothing,
                               Optional maximum As Integer? = Nothing) As IStore Implements IUniverseFactory.CreateStore
        If (minimum.HasValue AndAlso maximum.HasValue AndAlso maximum.Value < minimum.Value) Then
            Throw New ArgumentOutOfRangeException
        End If
        Dim storeData = New StoreData(statistics:=New Dictionary(Of String, Integer) From
                {
                    {PersistenceStatisticTypes.CurrentValue, value}
                })
        storeData.SetStatistic(PersistenceStatisticTypes.MinimumValue, minimum)
        storeData.SetStatistic(PersistenceStatisticTypes.MaximumValue, maximum)
        Dim storeId = UniverseData.Stores.Count
        UniverseData.Stores.Add(storeData)
        Return Store.FromId(UniverseData, storeId)
    End Function

    Public Function CreateItem(itemType As String) As IItem Implements IUniverseFactory.CreateItem
        Dim itemData = New ItemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {LegacyMetadataTypes.EntityType, itemType}
                }
            }
        Dim itemId = UniverseData.Items.Count
        UniverseData.Items.Add(itemData)
        Return Item.FromId(UniverseData, itemId)
    End Function
End Class
