Public Interface IAvatarStarSystemModel
    Inherits IAvatarPlaceModel
    Sub Enter()
    ReadOnly Property CanEnter As Boolean
    ReadOnly Property LegacyCurrent As IStarSystemModel
End Interface
