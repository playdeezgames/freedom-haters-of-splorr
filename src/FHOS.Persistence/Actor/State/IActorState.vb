Public Interface IActorState
    Property Location As ILocation
    'store relationship categories
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
End Interface
