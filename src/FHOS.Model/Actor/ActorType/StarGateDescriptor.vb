Friend Class StarGateDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarGate,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarGate, Hues.LightGreen), 1}
            },
            New Dictionary(Of String, Integer) From
            {
                {MapTypes.PlanetOrbit, 1}
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeStarGate)
    End Sub

    Private Shared Sub InitializeStarGate(actor As Persistence.IActor)
    End Sub
End Class
