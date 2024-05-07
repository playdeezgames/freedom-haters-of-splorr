Public Interface ILocationModel
    ReadOnly Property Exists As Boolean
    ReadOnly Property LocationType As ILocationTypeModel
    ReadOnly Property Actor As IActorModel
    ReadOnly Property Place As IPlaceModel
    ReadOnly Property HasDetails As Boolean
End Interface
