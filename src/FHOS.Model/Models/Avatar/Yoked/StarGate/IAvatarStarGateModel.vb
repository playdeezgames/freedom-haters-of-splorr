Public Interface IAvatarStarGateModel
    Inherits IAvatarYokedModel
    ReadOnly Property AvailableGates As IEnumerable(Of (Text As String, Item As IActorModel))
    Sub Enter(gate As IActorModel)
End Interface
