Friend Class ShipyardDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Shipyard,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Shipyard, Hues.Orange), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d4/4"}
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeShipyard)
    End Sub

    Private Shared Sub InitializeShipyard(actor As Persistence.IActor)
    End Sub
End Class
