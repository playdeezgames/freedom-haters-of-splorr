Friend Class PlaceProperties
    Inherits PlaceDataClient
    Implements IPlaceProperties

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public Property Name As String Implements IPlaceProperties.Name
        Get
            Return EntityData.Metadatas(MetadataTypes.Name)
        End Get
        Set(value As String)
            EntityData.Metadatas(MetadataTypes.Name) = value
        End Set
    End Property

    Public Property Map As IMap Implements IPlaceProperties.Map
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

    Public ReadOnly Property Identifier As String Implements IPlaceProperties.Identifier
        Get
            Return EntityData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property

    Public Property WormholeDestination As ILocation Implements IPlaceProperties.WormholeDestination
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

    Public Property Faction As IFaction Implements IPlaceProperties.Faction
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

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IPlaceProperties.Position
        Get
            Return (X, Y)
        End Get
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

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IPlaceProperties
        Throw New NotImplementedException()
    End Function
End Class
