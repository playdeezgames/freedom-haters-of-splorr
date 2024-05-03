Public Interface IAvatarPlaceModel
    ReadOnly Property Current As IPlaceModel
    Sub EnterStarSystem()
    ReadOnly Property CanEnterStarSystem As Boolean
    ReadOnly Property CanApproachStarVicinity As Boolean
    Sub ApproachStarVicinity()
End Interface
