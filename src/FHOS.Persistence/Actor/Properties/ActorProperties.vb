Friend Class ActorProperties
    Inherits ActorDataClient
    Implements IActorProperties

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorProperties
        Return New ActorProperties(universeData, id)
    End Function

    Public Property Group As IGroup Implements IActorProperties.Group
        Get
            Dim id = GetStatistic(StatisticTypes.GroupId)
            If id.HasValue Then
                Return Persistence.Group.FromId(UniverseData, id.Value)
            End If
            Return Nothing
        End Get
        Set(value As IGroup)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.GroupId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.GroupId)
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

    Public Property TargetActor As IActor Implements IActorProperties.TargetActor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(StatisticTypes.TargetActor))
        End Get
        Set(value As IActor)
            SetStatistic(StatisticTypes.TargetActor, value?.Id)
        End Set
    End Property

    Public Property IsSatellite As Boolean Implements IActorProperties.IsSatellite
        Get
            Return Flags(FlagTypes.IsSatellite)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsSatellite) = value
        End Set
    End Property

    Public Property IsWormhole As Boolean Implements IActorProperties.IsWormhole
        Get
            Return Flags(FlagTypes.IsWormhole)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsWormhole) = value
        End Set
    End Property

    Public Property Subtype As String Implements IActorProperties.Subtype
        Get
            Return GetMetadata(MetadataTypes.Subtype)
        End Get
        Set(value As String)
            SetMetadata(MetadataTypes.Subtype, value)
        End Set
    End Property

    Public Property IsPlanet As Boolean Implements IActorProperties.IsPlanet
        Get
            Return Flags(FlagTypes.IsPlanet)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsPlanet) = value
        End Set
    End Property

    Public Property HomePlanet As IGroup Implements IActorProperties.HomePlanet
        Get
            Return Persistence.Group.FromId(UniverseData, GetStatistic(StatisticTypes.HomePlanetActorId))
        End Get
        Set(value As IGroup)
            SetStatistic(StatisticTypes.HomePlanetActorId, value?.Id)
        End Set
    End Property

    Public Property IsPlanetVicinity As Boolean Implements IActorProperties.IsPlanetVicinity
        Get
            Return Flags(FlagTypes.IsPlanetVicinity)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsPlanetVicinity) = value
        End Set
    End Property

    Public Property SatelliteCount As Integer Implements IActorProperties.SatelliteCount
        Get
            Return If(GetStatistic(StatisticTypes.SatelliteCount), 0)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.SatelliteCount, value)
        End Set
    End Property

    Public Property PlanetCount As Integer Implements IActorProperties.PlanetCount
        Get
            Return If(GetStatistic(StatisticTypes.PlanetCount), 0)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.PlanetCount, value)
        End Set
    End Property

    Public Property IsStarSystem As Boolean Implements IActorProperties.IsStarSystem
        Get
            Return Flags(FlagTypes.IsStarSystem)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsStarSystem) = value
        End Set
    End Property

    Public Property IsStar As Boolean Implements IActorProperties.IsStar
        Get
            Return Flags(FlagTypes.IsStar)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsStar) = value
        End Set
    End Property

    Public Property IsStarVicinity As Boolean Implements IActorProperties.IsStarVicinity
        Get
            Return Flags(FlagTypes.IsStarVicinity)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsStarVicinity) = value
        End Set
    End Property

    Public Property IsSatelliteSection As Boolean Implements IActorProperties.IsSatelliteSection
        Get
            Return Flags(FlagTypes.IsSatelliteSection)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsSatelliteSection) = value
        End Set
    End Property

    Public Property IsPlanetSection As Boolean Implements IActorProperties.IsPlanetSection
        Get
            Return Flags(FlagTypes.IsPlanetSection)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.IsPlanetSection) = value
        End Set
    End Property
End Class
