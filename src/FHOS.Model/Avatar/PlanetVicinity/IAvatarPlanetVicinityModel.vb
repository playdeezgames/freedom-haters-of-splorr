Public Interface IAvatarPlanetVicinityModel
    ReadOnly Property Current As IPlanetVicinityModel
    ReadOnly Property CanApproach As Boolean
    Sub Approach()
End Interface
