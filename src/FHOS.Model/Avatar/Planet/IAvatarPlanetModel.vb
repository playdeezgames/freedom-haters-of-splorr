Public Interface IAvatarPlanetModel
    ReadOnly Property Current As IPlanetModel
    ReadOnly Property CanRefillOxygen As Boolean
    Sub RefillOxygen()
End Interface
