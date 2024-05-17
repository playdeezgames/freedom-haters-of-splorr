Imports FHOS.Data

Friend Class Map
    Inherits MapDataClient
    Implements IMap

    Protected Sub New(universeData As UniverseData, mapId As Integer)
        MyBase.New(universeData, mapId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, mapId As Integer?) As IMap
        If mapId.HasValue Then
            Return New Map(universeData, mapId.Value)
        End If
        Return Nothing
    End Function

    Public Property Name As String Implements IMap.Name
        Get
            Return EntityData.Metadatas(MetadataTypes.Name)
        End Get
        Set(value As String)
            EntityData.Metadatas(MetadataTypes.Name) = value
        End Set
    End Property

    Public ReadOnly Property Locations As IEnumerable(Of ILocation) Implements IMap.Locations
        Get
            Return EntityData.Locations.Select(Function(x) Location.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Size As (Columns As Integer, Rows As Integer) Implements IMap.Size
        Get
            Return (EntityData.Statistics(StatisticTypes.Columns), EntityData.Statistics(StatisticTypes.Rows))
        End Get
    End Property

    Public Function GetLocation(column As Integer, row As Integer) As ILocation Implements IMap.GetLocation
        If column < 0 OrElse row < 0 OrElse column >= EntityData.Statistics(StatisticTypes.Columns) OrElse row >= EntityData.Statistics(StatisticTypes.Rows) Then
            Return Nothing
        End If
        Return Location.FromId(UniverseData, EntityData.Locations(column + row * EntityData.Statistics(StatisticTypes.Columns)))
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
        Return Location.FromId(UniverseData, locationId)
    End Function
End Class
