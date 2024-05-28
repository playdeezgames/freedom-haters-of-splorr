Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Group As IGroup
    Property HomePlanet As IGroup
    Property CostumeType As String
    Property TargetActor As IActor

    'push to actor type
    Property IsSatellite As Boolean
    Property IsWormhole As Boolean
    Property Subtype As String
    Property IsPlanet As Boolean
    Property IsPlanetVicinity As Boolean
    Property SatelliteCount As Integer
    Property PlanetCount As Integer
    Property IsStarSystem As Boolean
    Property IsStar As Boolean
    Property IsStarVicinity As Boolean
    Property IsSatelliteSection As Boolean
    Property IsPlanetSection As Boolean
End Interface
