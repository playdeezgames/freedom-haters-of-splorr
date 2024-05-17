Public Interface IActor
    Inherits IEntity
    ReadOnly Property ActorType As String
    Property Location As ILocation
    Property Facing As Integer
    Property Name As String
    Property Costume As String
    Property LifeSupport As IStore
    Property MaximumFuel As Integer
    Property Fuel As Integer
    ReadOnly Property HasFuel As Boolean
    Property ConsumesFuel As Boolean
    ReadOnly Property HasTutorial As Boolean
    ReadOnly Property CurrentTutorial As String
    Sub DismissTutorial()
    Sub TriggerTutorial(tutorial As String)
    ReadOnly Property KnowsPlaces As Boolean
    ReadOnly Property KnownPlaces As IEnumerable(Of IPlace)
    Function KnowsPlacesOfType(placeType As String) As Boolean
    Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    Sub AddKnownPlace(place As IPlace)
    Property Jools As Integer
    Property MinimumJools As Integer
    Property Wallet As IStore
    Property Faction As IFaction
    Property HomePlanet As IPlace
    Property Interactor As IActor
    Property Interior As IMap
    Sub AddCrew(crew As IActor)
    Property Vessel As IActor
    ReadOnly Property HasCrew As Boolean
    ReadOnly Property Crew As IEnumerable(Of IActor)
End Interface
