Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New Dictionary(Of String, ActorTypeDescriptor) From
        {
            {
                PlayerShip,
                New ActorTypeDescriptor(
                    PlayerShip,
                    {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                    Hue.LightGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    spawnCount:=1,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
                    initializer:=AddressOf InitializePlayerShip)
            },
            {
                MilitaryShip,
                New MilitaryVesselDescriptor()
            },
            {
                Person,
                New ActorTypeDescriptor(
                    Person,
                    {ChrW(160), ChrW(160), ChrW(160), ChrW(160)},
                    Hue.LightGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    spawnCount:=25,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Air AndAlso x.Actor Is Nothing,
                    initializer:=AddressOf InitializePerson)
            }
        }

    Private Sub InitializePerson(actor As IActor)
        'TODO
    End Sub


    Private Sub InitializePlayerShip(actor As Persistence.IActor)
        actor.SetFlag(FlagTypes.IsAvatar)
        actor.Faction = actor.Universe.Factions.Single(Function(x) x.HasFlag(FlagTypes.LovesFreedom))
        actor.HomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Faction.Id = actor.Faction.Id))
        actor.Name = "(yer ship)"
        InitializePlayerShipInterior(actor)
        InitializePlayerShipCrew(actor)
    End Sub

    Private Sub InitializePlayerShipCrew(playerShip As IActor)
        Dim location = playerShip.
            Interior.
            GetLocation(PlayerShipInteriorColumns \ 2, PlayerShipInteriorRows \ 2)
        Dim actor = ActorTypes.Descriptors(ActorTypes.Person).CreateActor(location)
        playerShip.AddCrew(actor)
    End Sub

    Private Const PlayerShipInteriorColumns = 5
    Private Const PlayerShipInteriorRows = 5
    Private Sub InitializePlayerShipInterior(playerShip As IActor)
        Dim map = playerShip.Universe.CreateMap(
            MapTypes.Vessel,
            "Yer ship's interior",
            PlayerShipInteriorColumns,
            PlayerShipInteriorRows,
            LocationTypes.Air)
        playerShip.Interior = map
        For Each x In Enumerable.Range(0, PlayerShipInteriorColumns)
            map.GetLocation(x, 0).LocationType = LocationTypes.Bulkhead
            map.GetLocation(x, PlayerShipInteriorRows - 1).LocationType = LocationTypes.Bulkhead
        Next
        For Each y In Enumerable.Range(1, PlayerShipInteriorRows - 2)
            map.GetLocation(0, y).LocationType = LocationTypes.Bulkhead
            map.GetLocation(PlayerShipInteriorColumns - 1, y).LocationType = LocationTypes.Bulkhead
        Next
    End Sub
End Module
