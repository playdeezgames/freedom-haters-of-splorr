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
            })
    End Sub

    Private Shared Sub InitializePlayerShipCrew(ship As IActor)
        Dim location = ship.
            Interior.
            GetLocation(ship.Interior.Size.Columns \ 2, ship.Interior.Size.Rows \ 2)
        Dim actor = ActorTypes.Descriptors(ActorTypes.Person).CreateActor(location, "(you)")
        actor.Yokes.Store(YokeTypes.LifeSupport) = ship.Yokes.Store(YokeTypes.LifeSupport)
        ship.Family.AddChild(actor)
    End Sub

    Private Shared Sub InitializePlayerShipInterior(ship As IActor)
        Dim descriptor = MapTypes.Descriptors(MapTypes.Vessel)
        Dim map = descriptor.CreateMap(
            "Yer ship's interior",
            ship.Universe)
        ship.Interior = map
        For Each x In Enumerable.Range(0, descriptor.Size.Columns)
            map.GetLocation(x, 0).EntityType = LocationTypes.Bulkhead
            map.GetLocation(x, descriptor.Size.Rows - 1).EntityType = LocationTypes.Bulkhead
        Next
        For Each y In Enumerable.Range(1, descriptor.Size.Rows - 2)
            map.GetLocation(0, y).EntityType = LocationTypes.Bulkhead
            map.GetLocation(descriptor.Size.Columns - 1, y).EntityType = LocationTypes.Bulkhead
        Next
        map.GetLocation(descriptor.Size.Columns \ 2, 0).EntityType = LocationTypes.MakeDoor(CardinalDirections.North, False)
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
        InitializePlayerShipInterior(actor)
        InitializePlayerShipCrew(actor)
    End Sub

    Private Shared Sub InitializePlayerShipEquipment(actor As IActor)
        InitializePlayerShipLifeSupport(actor)
        InitializePlayerShipFuelSupply(actor)
    End Sub

    Private Shared Sub InitializePlayerShipFuelSupply(actor As IActor)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.FuelSupply, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(EquipSlots.FuelSupply, item)
    End Sub

    Private Shared Sub InitializePlayerShipLifeSupport(actor As IActor)
        Dim item = ItemTypes.MarkedDescriptor(ItemTypes.LifeSupport, Marks.MarkI).CreateItem(actor.Universe)
        actor.Equip(EquipSlots.LifeSupport, item)
    End Sub

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
