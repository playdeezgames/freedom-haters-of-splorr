Public Interface IActor
    Inherits IEntity
    ReadOnly Property KnownPlaces As IActorKnownPlaces
    ReadOnly Property Tutorial As IActorTutorial

    'Vessel
    Property Interior As IMap
    Sub AddCrew(crew As IActor)
    Property Vessel As IActor
    ReadOnly Property HasCrew As Boolean
    ReadOnly Property Crew As IEnumerable(Of IActor)
    Property LifeSupport As IStore
    Property FuelTank As IStore


    'Low Volatility Properties
    ReadOnly Property ActorType As String
    Property Name As String
    Property Faction As IFaction
    Property HomePlanet As IPlace
    Property Costume As String

    'High Volatility Properties
    Property Wallet As IStore
    Property Location As ILocation
    Property Facing As Integer
    Property Interactor As IActor
End Interface
