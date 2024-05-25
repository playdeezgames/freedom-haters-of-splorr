Imports FHOS.Persistence

Friend Class StarGateActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarGate,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarGate, Hues.LightGreen), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d10/10"}
            })
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim planet = actor.State.Location.Map.GetCenterLocation().Actor
        actor.Properties.Name = $"{planet.Properties.Group.Name} Star Gate"
        actor.Properties.Group = planet.Properties.Group.Group
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
