Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Group As IGroup
    Property HomePlanet As IGroup
    Property CostumeType As String
    Property CanSalvage As Boolean
    Property CanRefillOxygen As Boolean
    Property BuysScrap As Boolean
    Property CanRefuel As Boolean
    Property TargetActor As IActor
    Property IsSatellite As Boolean
    Property IsWormhole As Boolean
    Property Subtype As String
    Property IsPlanet As Boolean
    Property SectionName As String
    Property IsPlanetVicinity As Boolean
    Property SatelliteCount As Integer
    Property PlanetCount As Integer
    Property IsStarSystem As Boolean
    Property IsStar As Boolean
    Property IsStarVicinity As Boolean
End Interface
