Friend Class StarDockActorTypeDescriptor
    Inherits StationActorTypeDescriptor

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
            },
            canRefillOxygen:=True)
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        MyBase.Initialize(actor)

        actor.Properties.CanRefuel = True
    End Sub

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Properties.Group.Name}", Hues.Black)
            }
    End Function

    Protected Overrides Function MakeName(planet As Persistence.IActor) As String
        Return $"{planet.Properties.Name} Star Dock"

    End Function
End Class
