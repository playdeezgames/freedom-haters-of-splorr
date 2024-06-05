Imports FHOS.Persistence

Friend MustInherit Class StationActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Friend Sub New(
                  actorType As String,
                  costumeGenerator As IReadOnlyDictionary(Of String, Integer),
                  Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
                  Optional canRefillOxygen As Boolean = False,
                  Optional isStarGate As Boolean = False,
                  Optional buysScrap As Boolean = False,
                  Optional canRefuel As Boolean = False)
        MyBase.New(
            actorType,
            costumeGenerator,
            spawnRolls,
            canRefillOxygen:=canRefillOxygen,
            isStarGate:=isStarGate,
            buysScrap:=buysScrap,
            canRefuel:=canRefuel)
    End Sub

    Protected MustOverride Function MakeName(planet As IActor) As String

    Protected Overrides Sub Initialize(actor As IActor)
        Dim planet = actor.State.Location.Map.GetCenterLocation().Actor
        actor.EntityName = MakeName(planet)
        Dim faction = planet.GroupsOfCategory(CategoryTypes.Planet).Single.Parents.Single(Function(x) x.EntityType = GroupTypes.PlanetVicinity).Parents.Single(Function(x) x.EntityType = GroupTypes.Faction)
        actor.GroupCategory(faction) = CategoryTypes.Faction

        actor.State.Location.EntityType = LocationTypes.ActorAdjacent
        For Each neighbor In actor.State.Location.GetEmptyNeighborsOfType(LocationTypes.Void)
            neighbor.EntityType = LocationTypes.ActorAdjacent
        Next
    End Sub
End Class
