Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class MilitaryVesselActorTypeDescriptor
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
        actor.Properties.Group = RNG.FromGenerator(actor.Universe.Groups.ToDictionary(Function(x) x, Function(x) x.PlanetCount))
        actor.Properties.HomePlanet = RNG.FromEnumerable(actor.Universe.Actors.Where(Function(x) x.Properties.IsPlanet AndAlso x.Properties.SectionName = Grid3x3.Center AndAlso x.Properties.Group.Id = actor.Properties.Group.Id))
        actor.Properties.Name = $"{actor.Properties.Group.Name} Military Vessel"
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
