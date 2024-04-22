Public Interface IAvatarPlanetModel
    ReadOnly Property Current As IPlanetModel
    ReadOnly Property CanApproach As Boolean
    Sub Approach()
End Interface
