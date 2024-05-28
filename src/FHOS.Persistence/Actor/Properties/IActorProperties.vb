Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Group As IGroup
    Property HomePlanet As IGroup
    Property CostumeType As String
    Property TargetActor As IActor

    'push to actor type
    Property IsSatelliteSection As Boolean
    Property IsPlanetVicinity As Boolean
    Property IsPlanet As Boolean
    Property IsPlanetSection As Boolean
    Property IsWormhole As Boolean
    Property Subtype As String
    Property SatelliteCount As Integer
    Property PlanetCount As Integer
End Interface
