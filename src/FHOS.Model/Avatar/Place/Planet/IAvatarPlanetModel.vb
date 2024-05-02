Public Interface IAvatarPlanetModel
    Inherits IAvatarPlaceModel
    ReadOnly Property LegacyCurrent As IPlanetModel
    ReadOnly Property CanRefillOxygen As Boolean
    Sub RefillOxygen()
End Interface
