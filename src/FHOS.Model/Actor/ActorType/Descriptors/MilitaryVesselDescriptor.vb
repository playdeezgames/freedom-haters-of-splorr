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
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.Galaxy, "25d1"}
            })
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        actor.Properties.Faction = RNG.FromGenerator(actor.Universe.Factions.ToDictionary(Function(x) x, Function(x) x.PlanetCount))
        actor.Properties.HomePlanet = RNG.FromEnumerable(actor.Universe.GetPlacesOfType(PlaceTypes.Planet).Where(Function(x) x.Properties.Faction.Id = actor.Properties.Faction.Id))
        actor.Properties.Name = $"{actor.Properties.Faction.Name} Military Vessel"
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return DescribeInteractor(actor)
    End Function
End Class
