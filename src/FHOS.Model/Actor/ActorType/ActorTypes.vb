Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module ActorTypes
    Friend ReadOnly Player As String = NameOf(Player)
    Friend ReadOnly Military As String = NameOf(Military)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New Dictionary(Of String, ActorTypeDescriptor) From
        {
            {
                Player,
                New ActorTypeDescriptor(
                    Player,
                    {ChrW(128), ChrW(129), ChrW(130), ChrW(131)},
                    Hue.LightGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    spawnCount:=1,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
                    initializer:=AddressOf InitializePlayer)
            },
            {
                Military,
                New ActorTypeDescriptor(
                    Military,
                    {ChrW(132), ChrW(133), ChrW(134), ChrW(135)},
                    Hue.DarkGray,
                    maximumOxygen:=100,
                    maximumFuel:=100,
                    spawnCount:=25,
                    canSpawn:=Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
                    initializer:=AddressOf InitializeEnemy)
            }
        }

    Private Sub InitializeEnemy(actor As Persistence.IActor)
        actor.Faction = RNG.FromGenerator(actor.Universe.Factions.ToDictionary(Function(x) x, Function(x) x.PlanetCount))
        actor.HomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Faction.Id = actor.Faction.Id))
        actor.Fuel = RNG.FromRange(0, actor.MaximumFuel)
        actor.Oxygen = RNG.FromRange(0, actor.MaximumOxygen)
        actor.Name = $"{actor.Faction.Name} Military Vessel"
    End Sub

    Private Sub InitializePlayer(actor As Persistence.IActor)
        actor.SetFlag(FlagTypes.IsAvatar)
        actor.Faction = actor.Universe.Factions.Single(Function(x) x.HasFlag(FlagTypes.LovesFreedom))
        actor.HomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Faction.Id = actor.Faction.Id))
        actor.Name = "(you)"
    End Sub
End Module
