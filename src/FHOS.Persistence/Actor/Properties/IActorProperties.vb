Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Groups(groupType As String) As IGroup
    Property HomePlanet As IGroup
    Property CostumeType As String
    Property TargetActor As IActor
End Interface
