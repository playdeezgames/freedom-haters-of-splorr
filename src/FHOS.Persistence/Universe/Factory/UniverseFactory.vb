﻿Imports FHOS.Data

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

    Public Function CreateStarSystem(
                                    starSystemName As String,
                                    starType As String,
                                    x As Integer,
                                    y As Integer) As IPlace Implements IUniverseFactory.CreateStarSystem
        Dim placeData = New PlaceData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.Subtype, starType},
                    {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                    {MetadataTypes.PlaceType, PlaceTypes.StarSystem}
                },
                .Statistics = New Dictionary(Of String, Integer) From
                {
                    {StatisticTypes.X, x},
                    {StatisticTypes.Y, y}
                }
            }
        Dim starSystemId As Integer = UniverseData.Places.CreateOrRecycle(placeData)
        Return Place.FromId(UniverseData, starSystemId)
    End Function

    Public Function CreateWormhole(wormholeName As String) As IPlace Implements IUniverseFactory.CreateWormhole
        Dim placeData = New PlaceData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, wormholeName},
                    {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                    {MetadataTypes.PlaceType, PlaceTypes.Wormhole}
                }
            }
        Dim starSystemId As Integer = UniverseData.Places.CreateOrRecycle(placeData)
        Return Place.FromId(UniverseData, starSystemId)
    End Function

    Public Function CreateFaction(
                                 factionName As String,
                                 minimumPlanetCount As Integer,
                                 authority As Integer,
                                 standards As Integer,
                                 conviction As Integer) As IFaction Implements IUniverseFactory.CreateFaction
        Dim factionData = New FactionData With
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
        Return Faction.FromId(UniverseData, UniverseData.Factions.CreateOrRecycle(factionData))
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
End Class