Imports FHOS.Persistence

Friend MustInherit Class StationActorTypeDescriptor
    Inherits ActorTypeDescriptor
    ReadOnly statisticType As String

    Friend Sub New(
                  actorType As String,
                  costumeGenerator As IReadOnlyDictionary(Of String, Integer),
                  statisticType As String,
                  Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
                  Optional canRefillOxygen As Boolean = False,
                  Optional isStarGate As Boolean = False,
                  Optional canRefuel As Boolean = False,
                  Optional canTrade As Boolean = False)
        MyBase.New(
            actorType,
            costumeGenerator,
            spawnRolls,
            canRefillOxygen:=canRefillOxygen,
            isStarGate:=isStarGate,
            canRefuel:=canRefuel,
            canTrade:=canTrade)
        Me.statisticType = statisticType
    End Sub

    Protected MustOverride Function MakeName(planet As IActor) As String

    Protected Overrides Sub Initialize(actor As IActor)
        Dim planet = actor.Location.Map.GetCenterLocation().Actor
        actor.EntityName = MakeName(planet)
        Dim faction = planet.Yokes.Group(YokeTypes.Planet).SingleParent(GroupTypes.PlanetVicinity).SingleParent(GroupTypes.Faction)
        actor.Yokes.Group(YokeTypes.Faction) = faction

        actor.Location.EntityType = LocationTypes.ActorAdjacent
        For Each neighbor In actor.Location.GetEmptyNeighborsOfType(LocationTypes.Void)
            neighbor.EntityType = LocationTypes.ActorAdjacent
        Next
        Dim planetGroup = actor.Location.Map.YokedGroup(YokeTypes.Planet)
        Dim planetVicinityGroup = planetGroup.SingleParent(GroupTypes.PlanetVicinity)
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        actor.Yokes.Group(YokeTypes.StarSystem) = starSystemGroup
        planetGroup.Statistics(statisticType) += 1
        planetVicinityGroup.Statistics(statisticType) += 1
        starSystemGroup.Statistics(statisticType) += 1
    End Sub
End Class
