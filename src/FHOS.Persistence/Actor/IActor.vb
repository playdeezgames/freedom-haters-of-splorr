Public Interface IActor
    Inherits IEntity
    ReadOnly Property KnownPlaces As IActorKnownPlaces
    ReadOnly Property Tutorial As IActorTutorial
    ReadOnly Property Family As IActorFamily


    'Low Volatility Properties
    Property Interior As IMap
    ReadOnly Property ActorType As String
    Property Name As String
    Property Faction As IFaction
    Property HomePlanet As IPlace
    Property Costume As String

    'High Volatility Properties
    Property LifeSupport As IStore
    Property FuelTank As IStore
    Property Wallet As IStore
    Property Location As ILocation
    Property Facing As Integer
    Property Interactor As IActor
End Interface
