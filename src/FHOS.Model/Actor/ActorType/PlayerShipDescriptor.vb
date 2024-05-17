Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class PlayerShipDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            PlayerShip,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.MerchantVessel, Hues.LightGray), 1}
            },
            spawnCount:=1,
            canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializePlayerShip)
    End Sub


    Private Shared Sub InitializePlayerShip(ship As Persistence.IActor)
        If ship.Universe.Avatar.Actor Is Nothing Then
            ship.Universe.Avatar.Push(ship)
        End If
        ship.Properties.Faction = ship.Universe.Factions.Single(Function(x) x.Authority = 100 AndAlso x.Standards = 100 AndAlso x.Conviction = 100)
        ship.Properties.HomePlanet = RNG.FromEnumerable(ship.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Properties.Faction.Id = ship.Properties.Faction.Id))
        ship.Properties.Name = "(yer ship)"
        ship.State.LifeSupport = ship.Universe.Factory.CreateStore(PlayerShipMaximumOxygen, minimum:=0, maximum:=PlayerShipMaximumOxygen)
        ship.State.FuelTank = ship.Universe.Factory.CreateStore(PlayerShipMaximumFuel, minimum:=0, maximum:=PlayerShipMaximumFuel)
        InitializePlayerShipInterior(ship)
        InitializePlayerShipCrew(ship)
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

    Private Const PlayerShipMaximumOxygen = 100
    Private Const PlayerShipMaximumFuel = 100
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
    End Sub
End Class
