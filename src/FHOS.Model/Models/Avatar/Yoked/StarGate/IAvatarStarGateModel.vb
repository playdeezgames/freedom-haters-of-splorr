Public Interface IAvatarStarGateModel
    Inherits IAvatarYokedModel
    ReadOnly Property IsActive As Boolean
    ReadOnly Property AvailableGates As IEnumerable(Of (Text As String, Item As IActorModel))
    Sub Enter(gate As IActorModel)
    Sub Leave()
End Interface
