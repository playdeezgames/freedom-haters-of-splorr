Imports FHOS.Data

Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Protected Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, placeId As Integer?) As IPlace
        If placeId.HasValue Then
            Return New Place(universeData, placeId.Value)
        End If
        Return Nothing
    End Function

    Public Property Name As String Implements IPlace.Name
        Get
            Return EntityData.Metadatas(MetadataTypes.Name)
        End Get
        Set(value As String)
            EntityData.Metadatas(MetadataTypes.Name) = value
        End Set
    End Property

    Public Property Map As IMap Implements IPlace.Map
        Get
            Dim mapId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return Persistence.Map.FromId(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                EntityData.Statistics.Remove(StatisticTypes.MapId)
            Else
                EntityData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property

    Public ReadOnly Property Identifier As String Implements IPlace.Identifier
        Get
            Return EntityData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property

    Public ReadOnly Property PlaceType As String Implements IPlace.PlaceType
        Get
            Return EntityData.Metadatas(MetadataTypes.PlaceType)
        End Get
    End Property

    Public ReadOnly Property Parent As IPlace Implements IPlace.Parent
        Get
            Dim parentId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.ParentId, parentId) Then
                Return New Place(UniverseData, parentId)
            End If
            Return Nothing
        End Get
    End Property

    Protected Sub AddPlace(place As IPlace)
        EntityData.Descendants.Add(place.Id)
    End Sub

    Public ReadOnly Property Subtype As String Implements IPlace.Subtype
        Get
            Return EntityData.Metadatas(MetadataTypes.Subtype)
        End Get
    End Property

    Public Function CreateStarVicinity(x As Integer, y As Integer) As IPlace Implements IPlace.CreateStarVicinity
        Dim starVicinity = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.Subtype, Subtype},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.StarVicinity}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id},
                        {StatisticTypes.X, x},
                        {StatisticTypes.Y, y}
                    }
                }))
        AddPlace(starVicinity)
        Return starVicinity
    End Function

    Public Function CreatePlanetVicinity(
                                        planetName As String,
                                        planetType As String,
                                        x As Integer,
                                        y As Integer) As IPlace Implements IPlace.CreatePlanetVicinity
        Dim planetVicinity = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetName},
                        {MetadataTypes.Subtype, planetType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.PlanetVicinity}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id},
                        {StatisticTypes.X, x},
                        {StatisticTypes.Y, y}
                    }
                }))
        AddPlace(planetVicinity)
        Return planetVicinity
    End Function

    Public Function CreateStar() As IPlace Implements IPlace.CreateStar
        Dim star = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Star},
                        {MetadataTypes.Subtype, Subtype}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(star)
        Return star
    End Function

    Public Function CreatePlanet() As IPlace Implements IPlace.CreatePlanet
        Dim planet As IPlace = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Planet},
                        {MetadataTypes.Subtype, Subtype},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(planet)
        Return planet
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace Implements IPlace.CreateSatellite
        Dim satellite As IPlace = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.PlaceType, PlaceTypes.Satellite},
                        {MetadataTypes.Subtype, satelliteType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddSatellite(satellite)
        Return satellite
    End Function

    Private Sub AddSatellite(satellite As IPlace)
        EntityData.Descendants.Add(satellite.Id)
    End Sub

    Public Property WormholeDestination As ILocation Implements IPlace.WormholeDestination
        Get
            Dim locationId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.WormholeDestinationId, locationId) Then
                Return Location.FromId(UniverseData, locationId)
            End If
            Return Nothing
        End Get
        Set(value As ILocation)
            If value Is Nothing Then
                EntityData.Statistics.Remove(StatisticTypes.WormholeDestinationId)
                Return
            End If
            EntityData.Statistics(StatisticTypes.WormholeDestinationId) = value.Id
        End Set
    End Property

    Public Property Faction As IFaction Implements IPlace.Faction
        Get
            Return Persistence.Faction.FromId(UniverseData, GetStatistic(StatisticTypes.FactionId))
        End Get
        Set(value As IFaction)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.FactionId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.FactionId)
            End If
        End Set
    End Property

    Public Property PlanetCount As Integer Implements IPlace.PlanetCount
        Get
            Return If(GetStatistic(StatisticTypes.PlanetCount), 0)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.PlanetCount) = value
        End Set
    End Property

    Private ReadOnly Property X As Integer
        Get
            Return If(GetStatistic(StatisticTypes.X), 0)
        End Get
    End Property

    Private ReadOnly Property Y As Integer
        Get
            Return If(GetStatistic(StatisticTypes.Y), 0)
        End Get
    End Property

    Public Property SatelliteCount As Integer Implements IPlace.SatelliteCount
        Get
            Return If(GetStatistic(StatisticTypes.SatelliteCount), 0)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.SatelliteCount) = value
        End Set
    End Property

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IPlace.Position
        Get
            Return (X, Y)
        End Get
    End Property
End Class
