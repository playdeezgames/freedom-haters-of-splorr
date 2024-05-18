Friend Class ActorProperties
    Inherits ActorDataClient
    Implements IActorProperties

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorProperties
        Return New ActorProperties(universeData, id)
    End Function

    Public Property Faction As IFaction Implements IActorProperties.Faction
        Get
            Dim id = GetStatistic(StatisticTypes.FactionId)
            If id.HasValue Then
                Return Persistence.Faction.FromId(UniverseData, id.Value)
            End If
            Return Nothing
        End Get
        Set(value As IFaction)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.FactionId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.FactionId)
            End If
        End Set
    End Property

    Public Property HomePlanet As IPlace Implements IActorProperties.HomePlanet
        Get
            Dim id = 0
            If EntityData.Statistics.TryGetValue(StatisticTypes.HomePlanetId, id) Then
                Return Place.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IPlace)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.HomePlanetId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.HomePlanetId)
            End If
        End Set
    End Property

    Public Property Name As String Implements IActorProperties.Name
        Get
            Return GetMetadata(MetadataTypes.Name)
        End Get
        Set(value As String)
            SetMetadata(MetadataTypes.Name, value)
        End Set
    End Property

    Public Property Interior As IMap Implements IActorProperties.Interior
        Get
            Return Map.FromId(UniverseData, GetStatistic(StatisticTypes.MapId))
        End Get
        Set(value As IMap)
            SetStatistic(StatisticTypes.MapId, value?.Id)
        End Set
    End Property

    Public Property CostumeType As String Implements IActorProperties.CostumeType
        Get
            Return GetMetadata(MetadataTypes.Costume)
        End Get
        Set(value As String)
            SetMetadata(MetadataTypes.Costume, value)
        End Set
    End Property

    Public Property CanRefillOxygen As Boolean Implements IActorProperties.CanRefillOxygen
        Get
            Return Flags(FlagTypes.CanRefillOxygen)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.CanRefillOxygen) = value
        End Set
    End Property

    Public Property CanSalvage As Boolean Implements IActorProperties.CanSalvage
        Get
            Return Flags(FlagTypes.CanSalvage)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.CanSalvage) = value
        End Set
    End Property

    Public Property BuysScrap As Boolean Implements IActorProperties.BuysScrap
        Get
            Return Flags(FlagTypes.BuysScrap)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.BuysScrap) = value
        End Set
    End Property

    Public Property CanRefuel As Boolean Implements IActorProperties.CanRefuel
        Get
            Return Flags(FlagTypes.CanRefuel)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.CanRefuel) = value
        End Set
    End Property
End Class
