﻿Friend Class StarDockDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarDock,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarDock, Hues.Brown), 1}
            },
            New Dictionary(Of String, Integer) From
            {
                {MapTypes.PlanetOrbit, 1}
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeStarDock)
    End Sub

    Private Shared Sub InitializeStarDock(actor As Persistence.IActor)
    End Sub
End Class