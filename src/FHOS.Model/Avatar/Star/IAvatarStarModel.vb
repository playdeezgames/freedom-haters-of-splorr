Public Interface IAvatarStarModel
    ReadOnly Property Current As IStarModel
    ReadOnly Property CanRefillFuel As Boolean
    Sub Refuel()
End Interface
