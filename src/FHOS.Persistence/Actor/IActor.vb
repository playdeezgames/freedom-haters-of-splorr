Public Interface IActor
    Inherits IEntity
    ReadOnly Property ActorType As String
    Property Location As ILocation
    Property Facing As Integer
    Property Name As String

    Property MaximumOxygen As Integer
    Property Oxygen As Integer

    Property MaximumFuel As Integer
    Property Fuel As Integer
    ReadOnly Property HasFuel As Boolean

    ReadOnly Property HasTutorial As Boolean
    ReadOnly Property CurrentTutorial As String
    Sub DismissTutorial()
    Sub TriggerTutorial(tutorial As String)

    ReadOnly Property KnowsPlaces As Boolean
    ReadOnly Property KnownPlaces As IEnumerable(Of IPlace)
    Function KnowsPlacesOfType(placeType As String) As Boolean
    Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of IPlace)
    Sub AddKnownPlace(place As IPlace)

    Property Turn As Integer
    Property Jools As Integer
    Property MinimumJools As Integer
    Sub AddMessage(
                  header As String,
                  ParamArray lines As (Text As String, Hue As Integer)())
    Property Faction As IFaction
    ReadOnly Property Universe As IUniverse
    Property HomePlanet As IPlace
    Property Interactor As IActor
    Property Interior As IMap
    Sub AddCrew(crew As IActor)
    Property Vessel As IActor
    ReadOnly Property HasCrew As Boolean
    ReadOnly Property Crew As IEnumerable(Of IActor)
End Interface
