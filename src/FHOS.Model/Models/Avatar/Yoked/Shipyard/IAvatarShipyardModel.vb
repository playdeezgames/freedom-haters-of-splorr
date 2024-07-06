Public Interface IAvatarShipyardModel
    Inherits IAvatarYokedModel
    ReadOnly Property IsActive As Boolean
    Sub Leave()
    ReadOnly Property Specimen As IActorModel
End Interface
