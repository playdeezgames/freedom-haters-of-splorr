Friend Module InteractionTypes
    Friend ReadOnly Cancel As String = NameOf(Cancel)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly Refuel As String = NameOf(Refuel)
    Friend ReadOnly SalvageScrap As String = NameOf(SalvageScrap)
    Friend ReadOnly SellScrap As String = NameOf(SellScrap)
    Friend ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Friend ReadOnly EnterOrbit As String = NameOf(EnterOrbit)
    Friend ReadOnly LeaveOrbit As String = NameOf(LeaveOrbit)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionTypeDescriptor) =
        New List(Of InteractionTypeDescriptor) From
        {
            New CancelInteractionTypeDescriptor(),
            New RefillOxygenInteractionTypeDescriptor(),
            New SalvageScrapInteractionTypeDescriptor(),
            New SellScrapInteractionTypeDescriptor(),
            New RefuelInteractionTypeDescriptor(),
            New TransportInteractionTypeDescriptor(EnterWormhole, "Enter Wormhole", Function(x) x.Properties.IsWormhole),
            New TransportInteractionTypeDescriptor(LeaveOrbit, "Leave Orbit", Function(x) x.Properties.TargetActor.Properties.IsSatellite OrElse x.Properties.TargetActor.Properties.IsPlanet),
            New EnterInteriorInteractionTypeDescriptor(EnterOrbit, "Enter Orbit", Function(x) x.Properties.IsSatellite OrElse x.Properties.IsPlanet)
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
