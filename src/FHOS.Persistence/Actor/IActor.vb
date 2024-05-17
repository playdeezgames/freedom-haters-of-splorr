Public Interface IActor
    Inherits IEntity
    ReadOnly Property KnownPlaces As IActorKnownPlaces
    ReadOnly Property Tutorial As IActorTutorial
    ReadOnly Property Family As IActorFamily
    ReadOnly Property ActorType As String
    ReadOnly Property Properties As IActorProperties

    'IActorState
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
    Property Location As ILocation
    Property Facing As Integer
    Property Interactor As IActor
End Interface
