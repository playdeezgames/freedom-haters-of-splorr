Public Interface IActorState
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
    Property Location As ILocation
    Property Facing As Integer
    Property Interactor As IActor
End Interface
