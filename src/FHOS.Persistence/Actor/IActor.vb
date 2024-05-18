Public Interface IActor
    Inherits IEntity
    ReadOnly Property KnownPlaces As IActorKnownPlaces
    ReadOnly Property Tutorial As IActorTutorial
    ReadOnly Property Family As IActorFamily
    ReadOnly Property ActorType As String
    ReadOnly Property Properties As IActorProperties
    ReadOnly Property State As IActorState
End Interface
