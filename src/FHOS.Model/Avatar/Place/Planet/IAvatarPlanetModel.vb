Public Interface IAvatarPlanetModel
    Inherits IAvatarPlaceModel
    ReadOnly Property Current As IPlanetModel
    ReadOnly Property CanRefillOxygen As Boolean
    Sub RefillOxygen()
End Interface
