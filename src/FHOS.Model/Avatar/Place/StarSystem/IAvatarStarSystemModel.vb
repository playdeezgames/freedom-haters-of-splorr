Public Interface IAvatarStarSystemModel
    Inherits IAvatarPlaceModel
    Sub Enter()
    ReadOnly Property CanEnter As Boolean
    ReadOnly Property Current As IStarSystemModel
End Interface
