Imports System.Numerics
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
            maximumFuel:=100,
            spawnCount:=1,
            canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializePlayerShip)
    End Sub


    Private Shared Sub InitializePlayerShip(ship As Persistence.IActor)
        If ship.Universe.Avatar.Actor Is Nothing Then
            ship.Universe.Avatar.Push(ship)
        End If
        ship.Faction = ship.Universe.Factions.Single(Function(x) x.Authority = 100 AndAlso x.Standards = 100 AndAlso x.Conviction = 100)
        ship.HomePlanet = RNG.FromEnumerable(ship.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Faction.Id = ship.Faction.Id))
        ship.Name = "(yer ship)"
        ship.Wallet = ship.Universe.CreateStore(0, minimum:=0)
        ship.LifeSupport = ship.Universe.CreateStore(PlayerShipMaximumOxygen, minimum:=0, maximum:=PlayerShipMaximumOxygen)
        ship.ConsumesFuel = True
        InitializePlayerShipInterior(ship)
        InitializePlayerShipCrew(ship)
    End Sub

    Private Shared Sub InitializePlayerShipCrew(ship As IActor)
        Dim location = ship.
            Interior.
            GetLocation(PlayerShipInteriorColumns \ 2, PlayerShipInteriorRows \ 2)
        Dim actor = ActorTypes.Descriptors(ActorTypes.Person).CreateActor(location)
        actor.Wallet = ship.Wallet
        actor.LifeSupport = ship.LifeSupport
        ship.AddCrew(actor)
    End Sub

    Private Const PlayerShipInteriorColumns = 5
    Private Const PlayerShipInteriorRows = 5
    Private Const PlayerShipMaximumOxygen = 100
    Private Shared Sub InitializePlayerShipInterior(ship As IActor)
        Dim map = ship.Universe.CreateMap(
            MapTypes.Vessel,
            "Yer ship's interior",
            PlayerShipInteriorColumns,
            PlayerShipInteriorRows,
            LocationTypes.Air)
        ship.Interior = map
        For Each x In Enumerable.Range(0, PlayerShipInteriorColumns)
            map.GetLocation(x, 0).LocationType = LocationTypes.Bulkhead
            map.GetLocation(x, PlayerShipInteriorRows - 1).LocationType = LocationTypes.Bulkhead
        Next
        For Each y In Enumerable.Range(1, PlayerShipInteriorRows - 2)
            map.GetLocation(0, y).LocationType = LocationTypes.Bulkhead
            map.GetLocation(PlayerShipInteriorColumns - 1, y).LocationType = LocationTypes.Bulkhead
        Next
    End Sub
End Class
