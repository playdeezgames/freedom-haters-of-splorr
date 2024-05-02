Public Interface IAvatarStarModel
    Inherits IAvatarPlaceModel
    ReadOnly Property Current As IStarModel
    ReadOnly Property CanRefillFuel As Boolean
    Sub Refuel()
End Interface
