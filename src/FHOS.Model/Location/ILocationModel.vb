Public Interface ILocationModel
    ReadOnly Property Exists As Boolean
    ReadOnly Property Actor As IActorModel
    ReadOnly Property HasActor As Boolean

    ReadOnly Property LocationType As ILocationTypeModel 'eliminate
End Interface
