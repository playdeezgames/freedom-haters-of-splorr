Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class MilitaryVesselActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            MilitaryShip,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.MilitaryVessel, Hues.DarkGray), 1},
                {CostumeTypes.MakeCostume(CostumeTypes.MilitaryVessel, Hues.LightGray), 1},
                {CostumeTypes.MakeCostume(CostumeTypes.MilitaryVessel, Hues.White), 1},
                {CostumeTypes.MakeCostume(CostumeTypes.MilitaryVessel, Hues.Tan), 1},
                {CostumeTypes.MakeCostume(CostumeTypes.MilitaryVessel, Hues.Brown), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.Galaxy, "25d1"}
            })
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim faction = RNG.FromGenerator(actor.Universe.Groups.Where(Function(x) x.EntityType = GroupTypes.Faction).ToDictionary(Function(x) x, Function(x) x.Statistics(StatisticTypes.PlanetCount).Value))
        actor.YokedGroup(YokeTypes.Faction) = faction
        actor.YokedGroup(YokeTypes.HomePlanet) = RNG.FromEnumerable(faction.ChildrenOfType(GroupTypes.PlanetVicinity))
        actor.EntityName = $"{faction.EntityName} Military Vessel"
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
