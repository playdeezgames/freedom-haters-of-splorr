Friend Class ActorProperties
    Inherits ActorDataClient
    Implements IActorProperties

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorProperties
        Return New ActorProperties(universeData, id)
    End Function

    Public Property Interior As IMap Implements IActorProperties.Interior
        Get
            Return Map.FromId(UniverseData, GetStatistic(PersistenceStatisticTypes.MapId))
        End Get
        Set(value As IMap)
            SetStatistic(PersistenceStatisticTypes.MapId, value?.Id)
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
End Class
