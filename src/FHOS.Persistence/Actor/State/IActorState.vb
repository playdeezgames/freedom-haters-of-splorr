Public Interface IActorState
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
    Property Location As ILocation
    Property Interactor As IActor
    Property Scrap As Integer
    Property StarGate As IActor
End Interface
