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
                                    {StatisticTypes.MapId, Id},
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
