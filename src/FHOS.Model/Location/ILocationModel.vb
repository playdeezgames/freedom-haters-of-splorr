﻿Public Interface ILocationModel
    ReadOnly Property Exists As Boolean
    ReadOnly Property LocationType As ILocationTypeModel
    ReadOnly Property Actor As IActorModel
    ReadOnly Property StarSystem As IStarSystemModel
    ReadOnly Property Star As IStarModel
    ReadOnly Property HasDetails As Boolean
End Interface