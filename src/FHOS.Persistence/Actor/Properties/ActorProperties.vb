Friend Class ActorProperties
    Inherits ActorDataClient
    Implements IActorProperties

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorProperties
        Return New ActorProperties(universeData, id)
    End Function

    Public Property EntityName As String Implements IActorProperties.EntityName
        Get
            Return GetMetadata(LegacyMetadataTypes.Name)
        End Get
        Set(value As String)
            SetMetadata(LegacyMetadataTypes.Name, value)
        End Set
    End Property

    Public Property Interior As IMap Implements IActorProperties.Interior
        Get
            Return Map.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.MapId))
        End Get
        Set(value As IMap)
            SetStatistic(LegacyStatisticTypes.MapId, value?.Id)
        End Set
    End Property

    Public Property CostumeType As String Implements IActorProperties.CostumeType
        Get
            Return GetMetadata(LegacyMetadataTypes.Costume)
        End Get
        Set(value As String)
            SetMetadata(LegacyMetadataTypes.Costume, value)
        End Set
    End Property

    Public Property TargetActor As IActor Implements IActorProperties.TargetActor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.TargetActor))
        End Get
        Set(value As IActor)
            SetStatistic(LegacyStatisticTypes.TargetActor, value?.Id)
        End Set
    End Property

    Public Property HomePlanet As IGroup Implements IActorProperties.HomePlanet
        Get
            Return Persistence.Group.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.HomePlanetActorId))
        End Get
        Set(value As IGroup)
            SetStatistic(LegacyStatisticTypes.HomePlanetActorId, value?.Id)
        End Set
    End Property

    Public Property Groups(groupType As String) As IGroup Implements IActorProperties.Groups
        Get
            Dim groupId As Integer
            If EntityData.Groups.TryGetValue(groupType, groupId) Then
                Return Persistence.Group.FromId(UniverseData, groupId)
            End If
            Return Nothing
        End Get
        Set(value As IGroup)
            If value IsNot Nothing Then
                EntityData.Groups(groupType) = value.Id
            Else
                EntityData.Groups.Remove(groupType)
            End If
        End Set
    End Property
End Class
