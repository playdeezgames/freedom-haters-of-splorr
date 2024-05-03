Imports FHOS.Data

Public Class Universe
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(universeData As UniverseData)
        MyBase.New(universeData)
    End Sub

    Public Property Avatar As IActor Implements IUniverse.Avatar
        Get
            Dim avatarId As Integer
            If UniverseData.Statistics.TryGetValue(StatisticTypes.AvatarId, avatarId) Then
                Return New Actor(UniverseData, avatarId)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value IsNot Nothing Then
                UniverseData.Statistics(StatisticTypes.AvatarId) = value.Id
            Else
                UniverseData.Statistics.Remove(StatisticTypes.AvatarId)
            End If
        End Set
    End Property

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
            Return placeIds.Select(Function(x) New Place(UniverseData, x))
        End Get
    End Property

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
        Dim map = New Map(UniverseData, mapId)
        mapData.Locations = Enumerable.
                            Range(0, columns * rows).
                            Select(Function(x) map.CreateLocation(locationType, x Mod rows, x \ rows).Id).ToList
        Return map
    End Function

    Public Function CreateStarSystem(starSystemName As String, starType As String) As IPlace Implements IUniverse.CreateStarSystem
        Dim placeData = New PlaceData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.StarType, starType},
                    {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                    {MetadataTypes.PlaceType, PlaceTypes.StarSystem}
                }
            }
        Dim starSystemId As Integer = UniverseData.Places.CreateOrRecycle(placeData)
        Return New Place(UniverseData, starSystemId)
    End Function
End Class
