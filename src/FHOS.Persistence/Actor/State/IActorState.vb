Public Interface IActorState
    Property Location As ILocation
    'store relationship categories
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
    'actor relationship categories
    Property Interactor As IActor
    Property StarGate As IActor
End Interface
