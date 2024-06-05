Friend Class ActorProperties
    Inherits ActorDataClient
    Implements IActorProperties

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorProperties
        Return New ActorProperties(universeData, id)
    End Function

    Public Sub SetGroup(groupType As String, group As IGroup) Implements IActorProperties.SetGroup
        If group IsNot Nothing Then
            EntityData.Groups(groupType) = group.Id
        Else
            EntityData.Groups.Remove(groupType)
        End If
    End Sub

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

    Public Function GetGroup(groupType As String) As IGroup Implements IActorProperties.GetGroup
        Dim groupId As Integer
        If EntityData.Groups.TryGetValue(groupType, groupId) Then
            Return Persistence.Group.FromId(UniverseData, groupId)
        End If
        Return Nothing
    End Function
End Class
