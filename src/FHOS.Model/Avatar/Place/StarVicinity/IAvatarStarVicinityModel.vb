Public Interface IAvatarStarVicinityModel
    Inherits IAvatarPlaceModel
    ReadOnly Property LegacyCurrent As IStarVicinityModel
    ReadOnly Property CanApproach As Boolean
    Sub Approach()
End Interface
