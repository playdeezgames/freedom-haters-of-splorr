Friend Class TradingPostDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.TradingPost,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.TradingPost, Hues.Cyan), 1}
            },
            New Dictionary(Of String, Integer) From
            {
                {MapTypes.PlanetOrbit, 1}
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeTradingPost)
    End Sub

    Private Shared Sub InitializeTradingPost(actor As Persistence.IActor)
    End Sub
End Class
