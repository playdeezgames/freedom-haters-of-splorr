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
        actor.Properties.Groups(GroupTypes.Faction) = RNG.FromGenerator(actor.Universe.Groups.Where(Function(x) x.GroupType = GroupTypes.Faction).ToDictionary(Function(x) x, Function(x) x.Statistics(StatisticTypes.PlanetCount).Value))
        actor.Properties.HomePlanet = RNG.FromEnumerable(actor.Properties.Groups(GroupTypes.Faction).Children.Where(Function(x) x.GroupType = GroupTypes.PlanetVicinity))
        actor.Properties.Name = $"{actor.Properties.Groups(GroupTypes.Faction).Name} Military Vessel"
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
