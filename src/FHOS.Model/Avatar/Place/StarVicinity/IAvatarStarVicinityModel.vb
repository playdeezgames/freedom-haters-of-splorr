Public Interface IAvatarStarVicinityModel
    Inherits IAvatarPlaceModel
    ReadOnly Property Current As IStarVicinityModel
    ReadOnly Property CanApproach As Boolean
    Sub Approach()
End Interface
