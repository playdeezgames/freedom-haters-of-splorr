Imports FHOS.Data

Friend Class Map
    Inherits MapDataClient
    Implements IMap

    Sub New(worldData As UniverseData, mapId As Integer)
        MyBase.New(worldData, mapId)
    End Sub

    Public ReadOnly Property Universe As IUniverse Implements IMap.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Name As String Implements IMap.Name
        Get
            Return MapData.Metadatas(MetadataTypes.Name)
        End Get
        Set(value As String)
            MapData.Metadatas(MetadataTypes.Name) = value
        End Set
    End Property

    Public ReadOnly Property Id As Integer Implements IMap.Id
        Get
            Return MapId
        End Get
    End Property

    Public ReadOnly Property Locations As IEnumerable(Of ILocation) Implements IMap.Locations
        Get
            Return MapData.Locations.Select(Function(x) New Location(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Size As (Columns As Integer, Rows As Integer) Implements IMap.Size
        Get
            Return (MapData.Statistics(StatisticTypes.Columns), MapData.Statistics(StatisticTypes.Rows))
        End Get
    End Property

    Public Property Place As IPlace Implements IMap.Place
        Get
            Dim starSystemId As Integer
            If MapData.Statistics.TryGetValue(StatisticTypes.PlaceId, starSystemId) Then
                Return New Place(UniverseData, starSystemId)
            End If
            Return Nothing
        End Get
        Set(value As IPlace)
            If value IsNot Nothing Then
                MapData.Statistics(StatisticTypes.PlaceId) = value.Id
            Else
                MapData.Statistics.Remove(StatisticTypes.PlaceId)
            End If
        End Set
    End Property

    Public Property StarVicinity As IStarVicinity Implements IMap.StarVicinity
        Get
            Dim starVicinityId As Integer
            If MapData.Statistics.TryGetValue(StatisticTypes.StarVicinityId, starVicinityId) Then
                Return New StarVicinity(UniverseData, starVicinityId)
            End If
            Return Nothing
        End Get
        Set(value As IStarVicinity)
            If value IsNot Nothing Then
                MapData.Statistics(StatisticTypes.StarVicinityId) = value.Id
            Else
                MapData.Statistics.Remove(StatisticTypes.StarVicinityId)
            End If
        End Set
    End Property

    Public Property PlanetVicinity As IPlanetVicinity Implements IMap.PlanetVicinity
        Get
            Dim planetVicinityId As Integer
            If MapData.Statistics.TryGetValue(StatisticTypes.PlaceId, planetVicinityId) Then
                Return New PlanetVicinity(UniverseData, planetVicinityId)
            End If
            Return Nothing
        End Get
        Set(value As IPlanetVicinity)
            If value IsNot Nothing Then
                MapData.Statistics(StatisticTypes.PlaceId) = value.Id
            Else
                MapData.Statistics.Remove(StatisticTypes.PlaceId)
            End If
        End Set
    End Property

    Public Function GetLocation(column As Integer, row As Integer) As ILocation Implements IMap.GetLocation
        If column < 0 OrElse row < 0 OrElse column >= MapData.Statistics(StatisticTypes.Columns) OrElse row >= MapData.Statistics(StatisticTypes.Rows) Then
            Return Nothing
        End If
        Return New Location(UniverseData, MapData.Locations(column + row * MapData.Statistics(StatisticTypes.Columns)))
    End Function

    Public Function CreateLocation(locationType As String, column As Integer, row As Integer) As ILocation Implements IMap.CreateLocation
        Dim locationData = New LocationData With
                            {
                                .Statistics = New Dictionary(Of String, Integer) From
                                {
                                    {StatisticTypes.MapId, MapId},
                                    {StatisticTypes.Column, column},
                                    {StatisticTypes.Row, row}
                                },
                                .Metadatas = New Dictionary(Of String, String) From
                                {
                                    {MetadataTypes.LocationType, locationType}
                                }
                            }
        Dim locationId As Integer = UniverseData.Locations.CreateOrRecycle(locationData)
        Return New Location(UniverseData, locationId)
    End Function
End Class
