Public Interface IActor
    ReadOnly Property Id As Integer
    ReadOnly Property ActorType As String
    Property Location As ILocation
    Property Facing As Integer

    Property MaximumOxygen As Integer
    Property Oxygen As Integer

    Property MaximumFuel As Integer
    Property Fuel As Integer
    ReadOnly Property HasFuel As Boolean

    ReadOnly Property HasTutorial As Boolean
    ReadOnly Property CurrentTutorial As String
    Sub DismissTutorial()
    Sub TriggerTutorial(tutorial As String)

    ReadOnly Property KnowsStarSystems As Boolean
    ReadOnly Property KnownStarSystems As IEnumerable(Of IPlace)
    ReadOnly Property KnownStarVicinities As IEnumerable(Of IStarVicinity)
    ReadOnly Property KnownPlanetVicinities As IEnumerable(Of IPlanetVicinity)
    Sub AddPlace(place As IPlace)

    Sub SetFlag(flag As String)
    Function HasFlag(flag As String) As Boolean

    Property Turn As Integer
    Property Jools As Integer
    Property MinimumJools As Integer
    ReadOnly Property KnowsPlanetVicinities As Boolean
    ReadOnly Property KnowsStarVicinities As Boolean
    Sub AddMessage(
                  header As String,
                  ParamArray lines As (Text As String, Hue As Integer)())
    Sub AddStarVicinity(starVicinity As IStarVicinity)
    Sub AddPlanetVicinity(planetVicinity As IPlanetVicinity)
End Interface
