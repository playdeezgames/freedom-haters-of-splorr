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
            })
    End Sub

    Friend Overrides Function GetSpawnCount(mapType As String) As Integer
        If mapType = MapTypes.Galaxy Then
            Return 25
        End If
        Return MyBase.GetSpawnCount(mapType)
    End Function

    Protected Overrides Sub Initialize(actor As IActor)
        Dim faction = RNG.FromGenerator(actor.Universe.Groups.Where(Function(x) x.EntityType = GroupTypes.Faction).ToDictionary(Function(x) x, Function(x) x.Statistics(StatisticTypes.PlanetCount).Value))
        actor.Yokes.Group(YokeTypes.Faction) = faction
        actor.Yokes.Group(YokeTypes.HomePlanet) = RNG.FromEnumerable(faction.ChildrenOfType(GroupTypes.PlanetVicinity))
        actor.EntityName = $"{faction.EntityName} Military Vessel"
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
