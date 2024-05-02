Public Interface IAvatarPlanetVicinityModel
    Inherits IAvatarPlaceModel
    ReadOnly Property LegacyCurrent As IPlanetVicinityModel
    ReadOnly Property CanApproach As Boolean
    Sub Approach()
End Interface
