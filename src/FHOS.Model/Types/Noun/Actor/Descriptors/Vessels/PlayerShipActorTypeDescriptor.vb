Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class PlayerShipActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            PlayerShip,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.MerchantVessel, Hues.LightGray), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.Galaxy, "1d1"}
            },
            availableEquipSlots:=New List(Of String) From
            {
                Model.EquipSlots.PrimaryFuelSupply,
                Model.EquipSlots.PrimaryLifeSupport,
                Model.EquipSlots.Accessory(0),
                Model.EquipSlots.Accessory(1)
            })
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As IActor)
        If actor.Universe.Avatar.Actor Is Nothing Then
            actor.Universe.Avatar.SetActor(actor)
        End If
        Dim faction As IGroup = InitializeFaction(actor)
        Dim planet = InitializeHomePlanet(actor, faction)
        Dim starSystem As IGroup = InitializeStarSystem(actor, planet)
        actor.EntityName = "(yer ship)"
        InitializePlayerShipEquipment(actor)
    End Sub

    Private Function InitializeStarSystem(actor As IActor, planet As IGroup) As IGroup
        Dim starSystem = planet.SingleParent(GroupTypes.StarSystem)
        actor.SetReputation(starSystem, 100)
        Return starSystem
    End Function

    Private Shared Function InitializeHomePlanet(actor As IActor, faction As IGroup) As IGroup
        Dim candidates = faction.ChildrenOfType(GroupTypes.PlanetVicinity)
        Dim planet = RNG.FromEnumerable(candidates)
        actor.Yokes.Group(YokeTypes.HomePlanet) = planet
        actor.SetReputation(planet, 100)
        Return planet
    End Function

    Private Shared Function InitializeFaction(ByRef actor As IActor) As IGroup
        Dim sigmoFaction As IGroup = actor.Universe.Groups.Single(Function(x) x.EntityType = GroupTypes.Faction AndAlso x.Statistics(StatisticTypes.Authority).Value = 100 AndAlso x.Statistics(StatisticTypes.Standards).Value = 100 AndAlso x.Statistics(StatisticTypes.Conviction).Value = 100)
        actor.Yokes.Group(YokeTypes.Faction) = sigmoFaction
        actor.SetReputation(sigmoFaction, 100)
        Return sigmoFaction
    End Function

    Private Shared Sub InitializePlayerShipEquipment(actor As IActor)
        InitializePlayerShipLifeSupport(actor)
        InitializePlayerShipFuelSupply(actor)
    End Sub

    Private Shared Sub InitializePlayerShipFuelSupply(actor As IActor)
        actor.Yokes.Store(YokeTypes.FuelTank) = actor.Universe.Factory.CreateStore(0, 0, 0)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.FuelSupply, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(Model.EquipSlots.PrimaryFuelSupply, item)
    End Sub

    Private Shared Sub InitializePlayerShipLifeSupport(actor As IActor)
        actor.Yokes.Store(YokeTypes.LifeSupport) = actor.Universe.Factory.CreateStore(0, 0, 0)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.LifeSupport, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(Model.EquipSlots.PrimaryLifeSupport, item)
    End Sub

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
