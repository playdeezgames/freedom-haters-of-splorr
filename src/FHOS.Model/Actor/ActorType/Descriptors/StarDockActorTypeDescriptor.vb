Friend Class StarDockActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarDock,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarDock, Hues.Brown), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d1"}
            })
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        Dim map = actor.State.Location.Map
        Dim planet = map.GetCenterLocation().Actor
        actor.Properties.Name = $"{planet.Properties.Group.Name} Star Dock"
        actor.Properties.Group = planet.Properties.Group.Group
        actor.Properties.CanRefillOxygen = True
        actor.Properties.BuysScrap = True
        actor.Properties.CanRefuel = True
    End Sub

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Properties.Group.Name}", Hues.Black)
            }
    End Function
End Class
