Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Groups(groupType As String) As IGroup
    Property Group As IGroup
    Property HomePlanet As IGroup
    Property StarSystem As IGroup
    Property PlanetVicinity As IGroup
    Property Planet As IGroup

    Property CostumeType As String
    Property TargetActor As IActor
End Interface
