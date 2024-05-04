Public Interface IAvatarPlaceModel
    ReadOnly Property Current As IPlaceModel
    ReadOnly Property CanApproachPlanetVicinity As Boolean
    Sub ApproachPlanetVicinity()
    ReadOnly Property CanRefillOxygen As Boolean
    Sub RefillOxygen()
    ReadOnly Property Verbs As IEnumerable(Of String)
    Sub DoVerb(verbType As String)

End Interface
