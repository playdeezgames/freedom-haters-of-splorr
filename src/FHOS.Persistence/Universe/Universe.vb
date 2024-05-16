Imports FHOS.Data

Public Class Universe
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(universeData As UniverseData)
        MyBase.New(universeData)
    End Sub

    Public ReadOnly Property Messages As IMessages Implements IUniverse.Messages
        Get
            Return New Messages(UniverseData.Messages)
        End Get
    End Property

    Public ReadOnly Property Places As IEnumerable(Of IPlace) Implements IUniverse.Places
        Get
            Dim placeIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Places.Entities.Count))
            For Each recycledId In UniverseData.Places.Recycled
                placeIds.Remove(recycledId)
            Next
            Return placeIds.Select(Function(x) Place.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Factions As IEnumerable(Of IFaction) Implements IUniverse.Factions
        Get
            Dim factionIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Factions.Entities.Count))
            For Each recycledId In UniverseData.Factions.Recycled
                factionIds.Remove(recycledId)
            Next
            Return factionIds.Select(Function(x) Faction.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Actors As IEnumerable(Of IActor) Implements IUniverse.Actors
        Get
            Dim actorIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Actors.Entities.Count))
            For Each recycledId In UniverseData.Actors.Recycled
                actorIds.Remove(recycledId)
            Next
            Return actorIds.Select(Function(x) Actor.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Avatar As IActor Implements IUniverse.Avatar
        Get
            Dim avatarId As Integer
            If UniverseData.Avatars.TryPeek(avatarId) Then
                Return Actor.FromId(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
    End Property

    Public Property Turn As Integer Implements IUniverse.Turn
        Get
            Return UniverseData.Statistics(StatisticTypes.Turn)
        End Get
        Set(value As Integer)
            UniverseData.Statistics(StatisticTypes.Turn) = value
        End Set
    End Property

    Public Sub PushAvatar(avatar As IActor) Implements IUniverse.PushAvatar
        UniverseData.Avatars.Push(avatar.Id)
    End Sub

    Public Function PopAvatar() As IActor Implements IUniverse.PopAvatar
        Return Actor.FromId(UniverseData, UniverseData.Avatars.Pop())
    End Function

    Public Function CreateMap(mapType As String, mapName As String, columns As Integer, rows As Integer, locationType As String) As IMap Implements IUniverse.CreateMap
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
                                    y As Integer) As IPlace Implements IUniverse.CreateStarSystem
        Dim placeData = New PlaceData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.StarType, starType},
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

    Public Function CreateWormhole(wormholeName As String) As IPlace Implements IUniverse.CreateWormhole
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
                                 conviction As Integer) As IFaction Implements IUniverse.CreateFaction
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

    Public Function GetPlacesOfType(placeType As String) As IEnumerable(Of IPlace) Implements IUniverse.GetPlacesOfType
        Return Places.Where(Function(x) x.PlaceType = placeType)
    End Function

    Public Function CreateStore(value As Integer, Optional minimum As Integer? = Nothing, Optional maximum As Integer? = Nothing) As IStore Implements IUniverse.CreateStore
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
