Friend Class StarDockDescriptor
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
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeStarDock)
    End Sub

    Private Shared Sub InitializeStarDock(actor As Persistence.IActor)
        actor.Properties.CanRefillOxygen = True
    End Sub
End Class
