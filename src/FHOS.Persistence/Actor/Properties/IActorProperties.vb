Public Interface IActorProperties
    Property Interior As IMap
    Property Name As String
    Property Faction As IFaction
    Property HomePlanet As IActor
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
End Interface
