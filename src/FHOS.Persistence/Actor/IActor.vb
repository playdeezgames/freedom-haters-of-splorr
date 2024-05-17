Public Interface IActor
    Inherits IEntity
    ReadOnly Property KnownPlaces As IActorKnownPlaces
    ReadOnly Property Tutorial As IActorTutorial

    'Crew
    Sub AddCrew(crew As IActor)
    ReadOnly Property HasCrew As Boolean
    ReadOnly Property AllCrew As IEnumerable(Of IActor)
    Property Parent As IActor


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
