Imports System.Transactions
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
        Return Map
    End Function

    Public Function CreateStarSystem(starSystemName As String, starType As String) As IStarSystem Implements IUniverse.CreateStarSystem
        Dim starSystemData = New StarSystemData With
            {
                .Metadatas = New Dictionary(Of String, String) From
                {
                    {MetadataTypes.Name, starSystemName},
                    {MetadataTypes.StarType, starType}
                }
            }
        Dim starSystemId As Integer = UniverseData.StarSystems.CreateOrRecycle(starSystemData)
        Return New StarSystem(UniverseData, starSystemId)
    End Function
End Class
