Public Interface IAvatarStarModel
    ReadOnly Property Current As IPlaceModel
    ReadOnly Property CanRefillFuel As Boolean
    Sub Refuel()
End Interface
