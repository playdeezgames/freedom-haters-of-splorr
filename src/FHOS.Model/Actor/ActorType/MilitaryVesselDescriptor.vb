Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class MilitaryVesselDescriptor
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
            },
            maximumFuel:=100,
            spawnCount:=25,
            canSpawn:=Function(x) x.LocationType = Void AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializeMilitaryShip)
    End Sub
    Private Shared Sub InitializeMilitaryShip(actor As Persistence.IActor)
        actor.Faction = RNG.FromGenerator(actor.Universe.Factions.ToDictionary(Function(x) x, Function(x) x.PlanetCount))
        actor.HomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Faction.Id = actor.Faction.Id))
        actor.Fuel = RNG.FromRange(0, actor.MaximumFuel)
        actor.Name = $"{actor.Faction.Name} Military Vessel"
        actor.ConsumesFuel = True
    End Sub
End Class
