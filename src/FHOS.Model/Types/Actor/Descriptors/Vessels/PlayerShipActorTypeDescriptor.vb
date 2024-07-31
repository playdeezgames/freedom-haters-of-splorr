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
            availableEquipSlots:=New Dictionary(Of String, Boolean) From
            {
                {Model.EquipSlots.FuelSupply, True},
                {Model.EquipSlots.LifeSupport, True},
                {Model.EquipSlots.Accessory, False}
            })
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As IActor)
        If actor.Universe.Avatar.Actor Is Nothing Then
            actor.Universe.Avatar.Push(actor)
        End If
        Dim sigmoFaction = actor.Universe.Groups.Single(Function(x) x.EntityType = GroupTypes.Faction AndAlso x.Statistics(StatisticTypes.Authority).Value = 100 AndAlso x.Statistics(StatisticTypes.Standards).Value = 100 AndAlso x.Statistics(StatisticTypes.Conviction).Value = 100)
        actor.Yokes.Group(YokeTypes.Faction) = sigmoFaction

        Dim planetCandidates = sigmoFaction.ChildrenOfType(GroupTypes.PlanetVicinity)
        actor.Yokes.Group(YokeTypes.HomePlanet) = RNG.FromEnumerable(planetCandidates)
        actor.EntityName = "(yer ship)"
        InitializePlayerShipEquipment(actor)
    End Sub

    Private Shared Sub InitializePlayerShipEquipment(actor As IActor)
        InitializePlayerShipLifeSupport(actor)
        InitializePlayerShipFuelSupply(actor)
    End Sub

    Private Shared Sub InitializePlayerShipFuelSupply(actor As IActor)
        actor.Yokes.Store(YokeTypes.FuelTank) = actor.Universe.Factory.CreateStore(0, 0, 0)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.FuelSupply, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(Model.EquipSlots.FuelSupply, item)
    End Sub

    Private Shared Sub InitializePlayerShipLifeSupport(actor As IActor)
        actor.Yokes.Store(YokeTypes.LifeSupport) = actor.Universe.Factory.CreateStore(0, 0, 0)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.LifeSupport, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(Model.EquipSlots.LifeSupport, item)
    End Sub

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
