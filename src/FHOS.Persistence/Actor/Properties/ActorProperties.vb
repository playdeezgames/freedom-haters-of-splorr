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
                Select Case value.GroupType
                    Case "Faction"
                        EntityData.Statistics(StatisticTypes.GroupId) = value.Id
                    Case Else
                        Throw New ArgumentException(value.GroupType)
                End Select
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

    Public Property TargetActor As IActor Implements IActorProperties.TargetActor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(StatisticTypes.TargetActor))
        End Get
        Set(value As IActor)
            SetStatistic(StatisticTypes.TargetActor, value?.Id)
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

    Public Property StarSystem As IGroup Implements IActorProperties.StarSystem
        Get
            Return Persistence.Group.FromId(UniverseData, GetStatistic(StatisticTypes.StarSystemId))
        End Get
        Set(value As IGroup)
            SetStatistic(StatisticTypes.StarSystemId, value?.Id)
        End Set
    End Property

    Public Property PlanetVicinity As IGroup Implements IActorProperties.PlanetVicinity
        Get
            Return Persistence.Group.FromId(UniverseData, GetStatistic(StatisticTypes.PlanetVicinityId))
        End Get
        Set(value As IGroup)
            SetStatistic(StatisticTypes.PlanetVicinityId, value?.Id)
        End Set
    End Property

    Public Property Planet As IGroup Implements IActorProperties.Planet
        Get
            Return Persistence.Group.FromId(UniverseData, EntityData.Groups("Planet"))
        End Get
        Set(value As IGroup)
            EntityData.Groups("Planet") = value.Id
        End Set
    End Property

    Public Property Satellite As IGroup Implements IActorProperties.Satellite
        Get
            Return Persistence.Group.FromId(UniverseData, EntityData.Groups("Satellite"))
        End Get
        Set(value As IGroup)
            EntityData.Groups("Satellite") = value.Id
        End Set
    End Property
End Class
