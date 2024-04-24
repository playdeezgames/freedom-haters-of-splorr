Public Interface IActor
    ReadOnly Property Id As Integer
    Property Location As ILocation
    ReadOnly Property ActorType As String
    Property MaximumOxygen As Integer
    Property Oxygen As Integer
    Property MaximumFuel As Integer
    Property Fuel As Integer
    Property Facing As Integer
    ReadOnly Property HasTutorial As Boolean
    ReadOnly Property CurrentTutorial As String
    ReadOnly Property KnowsStarSystems As Boolean
    ReadOnly Property KnownStarSystems As IEnumerable(Of IStarSystem)
    Sub DismissTutorial()
    Sub SetFlag(flag As String)
    Sub AddStarSystem(starSystem As IStarSystem)
    Function HasFlag(flag As String) As Boolean
End Interface
