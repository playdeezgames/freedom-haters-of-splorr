Public Interface IAvatarStarModel
    Inherits IAvatarPlaceModel
    ReadOnly Property LegacyCurrent As IStarModel
    ReadOnly Property CanRefillFuel As Boolean
    Sub Refuel()
End Interface
