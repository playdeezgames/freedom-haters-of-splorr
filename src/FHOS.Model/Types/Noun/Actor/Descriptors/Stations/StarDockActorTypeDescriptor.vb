Imports SPLORR.Game

Friend Class StarDockActorTypeDescriptor
    Inherits StationActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarDock,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarDock, Hues.Brown), 1}
            },
            StatisticTypes.StarDockCount,
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d1"}
            },
            canRefillOxygen:=True,
            canRefuel:=True)
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.Group(YokeTypes.Faction).EntityName}", Hues.LightGray)
            }
    End Function

    Protected Overrides Function MakeName(planet As Persistence.IActor) As String
        Return $"{planet.EntityName} Star Dock"
    End Function

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        MyBase.Initialize(actor)
        InitializeDeliveryItem(actor)
    End Sub

    Private Shared Sub InitializeDeliveryItem(actor As Persistence.IActor)
        Dim deliveryItem = actor.Universe.Factory.CreateItem(ItemTypes.Delivery)
        actor.Inventory.Add(deliveryItem)
        Dim planet = actor.Yokes.Group(YokeTypes.Planet)
        deliveryItem.Statistics(StatisticTypes.OriginPlanetId) = planet.Id
        Dim planetVicinity = planet.Parents.Single(Function(x) x.EntityType = GroupTypes.PlanetVicinity)
        Dim faction = planetVicinity.Parents.Single(Function(x) x.EntityType = GroupTypes.Faction)
        Dim candidates = faction.
            ChildrenOfType(GroupTypes.PlanetVicinity).
            Where(Function(x) x.Id <> planetVicinity.Id).
            Select(Function(x) x.Children.Single(Function(y) y.EntityType = GroupTypes.Planet))
        Dim destination = RNG.FromEnumerable(candidates)
        deliveryItem.SetDestinationPlanet(destination)
        deliveryItem.SetJoolsReward(10)
        deliveryItem.SetReputationBonus(1)
        deliveryItem.SetReputationPenalty(-5)
    End Sub
End Class
