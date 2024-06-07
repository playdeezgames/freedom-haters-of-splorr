Friend Class ActorState
    Inherits ActorDataClient
    Implements IActorState

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public Property Location As ILocation Implements IActorState.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.Statistics(LegacyStatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(LegacyStatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(LegacyStatisticTypes.LocationId) = value.Id
                value.Actor = Actor.FromId(UniverseData, Id)
                ActorTutorial.FromId(UniverseData, Id).Add(value.Tutorial)
            End If
        End Set
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorState
        Return New ActorState(universeData, id)
    End Function
End Class
