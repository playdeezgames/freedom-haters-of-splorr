Imports FHOS.Persistence

Friend MustInherit Class StationActorTypeDescriptor
    Inherits ActorTypeDescriptor
    ReadOnly statisticType As String

    Friend Sub New(
                  actorType As String,
                  costumeGenerator As IReadOnlyDictionary(Of String, Integer),
                  statisticType As String,
                  Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
                  Optional flags As IEnumerable(Of String) = Nothing,
                  Optional canRefuel As Boolean = False,
                  Optional canTrade As Boolean = False,
                  Optional canUpgradeShip As Boolean = False)
        MyBase.New(
            actorType,
            costumeGenerator,
            spawnRolls,
            flags:=flags,
            canRefuel:=canRefuel,
            canTrade:=canTrade,
            canUpgradeShip:=canUpgradeShip)
        Me.statisticType = statisticType
    End Sub

    Protected MustOverride Function MakeName(planet As IActor) As String

    Protected Overrides Sub Initialize(actor As IActor)
        Dim planetActor = actor.Location.Map.GetCenterLocation().Actor
        actor.EntityName = MakeName(planetActor)
        Dim planetVicinity = planetActor.Yokes.Group(YokeTypes.Planet).SingleParent(GroupTypes.PlanetVicinity)
        Dim faction = planetVicinity.SingleParent(GroupTypes.Faction)
        actor.Yokes.Group(YokeTypes.Faction) = faction
        actor.Statistics(StatisticTypes.TechLevel) = planetVicinity.Statistics(StatisticTypes.TechLevel)

        actor.Location.EntityType = LocationTypes.ActorAdjacent
        For Each neighbor In actor.Location.GetEmptyOrdinalNeighborsOfType(LocationTypes.Void)
            neighbor.EntityType = LocationTypes.ActorAdjacent
        Next
        Dim planetGroup = actor.Location.Map.YokedGroup(YokeTypes.Planet)
        Dim planetVicinityGroup = planetGroup.SingleParent(GroupTypes.PlanetVicinity)
        actor.Yokes.Group(YokeTypes.Planet) = planetGroup
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        actor.Yokes.Group(YokeTypes.StarSystem) = starSystemGroup
        planetGroup.Statistics(statisticType) += 1
        planetVicinityGroup.Statistics(statisticType) += 1
        starSystemGroup.Statistics(statisticType) += 1
    End Sub
End Class
