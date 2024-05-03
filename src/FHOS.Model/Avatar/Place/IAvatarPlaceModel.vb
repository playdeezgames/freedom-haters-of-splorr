Public Interface IAvatarPlaceModel
    ReadOnly Property Current As IPlaceModel
    Sub EnterStarSystem()
    ReadOnly Property CanEnterStarSystem As Boolean
    ReadOnly Property CanApproachStarVicinity As Boolean
    Sub ApproachStarVicinity()
    ReadOnly Property CanRefillFuel As Boolean
    Sub Refuel()
    ReadOnly Property CanApproachPlanetVicinity As Boolean
    Sub ApproachPlanetVicinity()
End Interface
