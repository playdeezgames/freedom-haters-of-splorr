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
    ReadOnly Property KnownStarSystems As IEnumerable(Of IStarSystem)
    Sub AddStarSystem(starSystem As IStarSystem)

    Sub SetFlag(flag As String)
    Function HasFlag(flag As String) As Boolean

    Property Turn As Integer
    Property Jools As Integer

    Sub AddMessage(
                  header As String,
                  ParamArray lines As (Text As String, Hue As Integer)())
    Sub AddStarVicinity(starVicinity As IStarVicinity)
End Interface
