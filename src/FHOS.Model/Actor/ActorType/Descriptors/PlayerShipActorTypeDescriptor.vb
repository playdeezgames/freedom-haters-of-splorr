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
            Properties.
            Interior.
            GetLocation(ship.Properties.Interior.Size.Columns \ 2, ship.Properties.Interior.Size.Rows \ 2)
        Dim actor = ActorTypes.Descriptors(ActorTypes.Person).CreateActor(location)
        actor.State.LifeSupport = ship.State.LifeSupport
        ship.Family.AddChild(actor)
    End Sub

    Private Const PlayerShipMaximumOxygen = 1000
    Private Const PlayerShipMaximumFuel = 1000
    Private Shared Sub InitializePlayerShipInterior(ship As IActor)
        Dim descriptor = MapTypes.Descriptors(MapTypes.Vessel)
        Dim map = descriptor.CreateMap(
            "Yer ship's interior",
            ship.Universe)
        ship.Properties.Interior = map
        For Each x In Enumerable.Range(0, descriptor.Size.Columns)
            map.GetLocation(x, 0).LocationType = LocationTypes.Bulkhead
            map.GetLocation(x, descriptor.Size.Rows - 1).LocationType = LocationTypes.Bulkhead
        Next
        For Each y In Enumerable.Range(1, descriptor.Size.Rows - 2)
            map.GetLocation(0, y).LocationType = LocationTypes.Bulkhead
            map.GetLocation(descriptor.Size.Columns - 1, y).LocationType = LocationTypes.Bulkhead
        Next
        map.GetLocation(descriptor.Size.Columns \ 2, 0).LocationType = LocationTypes.MakeDoor(CardinalDirections.North, False)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As IActor)
        If actor.Universe.Avatar.Actor Is Nothing Then
            actor.Universe.Avatar.Push(actor)
        End If
        actor.Properties.Faction = actor.Universe.Factions.Single(Function(x) x.Authority = 100 AndAlso x.Standards = 100 AndAlso x.Conviction = 100)
        actor.Properties.LegacyHomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Properties.Faction.Id = actor.Properties.Faction.Id))
        actor.Properties.HomePlanet = RNG.FromEnumerable(actor.Universe.Actors.Where(Function(x) x.Properties.IsPlanet AndAlso x.Properties.SectionName = Grid3x3.Center AndAlso x.Properties.Faction.Id = actor.Properties.Faction.Id))
        actor.Properties.Name = "(yer ship)"
        actor.State.LifeSupport = actor.Universe.Factory.CreateStore(PlayerShipMaximumOxygen, minimum:=0, maximum:=PlayerShipMaximumOxygen)
        actor.State.FuelTank = actor.Universe.Factory.CreateStore(PlayerShipMaximumFuel, minimum:=0, maximum:=PlayerShipMaximumFuel)
        InitializePlayerShipInterior(actor)
        InitializePlayerShipCrew(actor)
    End Sub

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
