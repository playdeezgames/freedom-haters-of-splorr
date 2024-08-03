Friend Module InteractionTypes
    Friend ReadOnly AcceptDeliveryMission As String = NameOf(AcceptDeliveryMission)
    Friend ReadOnly Approach As String = NameOf(Approach)
    Friend ReadOnly Cancel As String = NameOf(Cancel)
    Friend ReadOnly EnterOrbit As String = NameOf(EnterOrbit)
    Friend ReadOnly EnterShipyard As String = NameOf(EnterShipyard)
    Friend ReadOnly EnterStarGate As String = NameOf(EnterStarGate)
    Friend ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Friend ReadOnly LeaveOrbit As String = NameOf(LeaveOrbit)
    Friend ReadOnly LeavePlanetVicinity As String = NameOf(LeavePlanetVicinity)
    Friend ReadOnly LeaveStarSystem As String = NameOf(LeaveStarSystem)
    Friend ReadOnly LeaveStarVicinity As String = NameOf(LeaveStarVicinity)
    Friend ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Friend ReadOnly Refuel As String = NameOf(Refuel)
    Friend ReadOnly SalvageScrap As String = NameOf(SalvageScrap)
    Friend ReadOnly Trade As String = NameOf(Trade)
    Friend ReadOnly UseFuelScoop As String = NameOf(UseFuelScoop)
    Friend ReadOnly UseAirScoop As String = NameOf(UseAirScoop)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, InteractionTypeDescriptor) =
        New List(Of InteractionTypeDescriptor) From
        {
            New CancelInteractionTypeDescriptor(),
            New EnterStarGateInteractionTypeDescriptor(),
            New RefillOxygenInteractionTypeDescriptor(),
            New SalvageScrapInteractionTypeDescriptor(),
            New RefuelInteractionTypeDescriptor(),
            New TransportInteractionTypeDescriptor(EnterWormhole, "Enter Wormhole", Function(x) x.Descriptor.IsWormhole),
            New TransportInteractionTypeDescriptor(LeaveOrbit, "Leave Orbit", Function(x) x.Yokes.Actor(YokeTypes.Target).Descriptor.IsSatellite OrElse x.Yokes.Actor(YokeTypes.Target).Descriptor.IsPlanet),
            New TransportInteractionTypeDescriptor(LeavePlanetVicinity, "Leave Planet Vicinity", Function(x) x.Yokes.Actor(YokeTypes.Target).Descriptor.IsPlanetVicinity),
            New TransportInteractionTypeDescriptor(LeaveStarSystem, "Leave Star System", Function(x) x.Yokes.Actor(YokeTypes.Target).Descriptor.IsStarSystem),
            New TransportInteractionTypeDescriptor(LeaveStarVicinity, "Leave Star Vicinity", Function(x) x.Yokes.Actor(YokeTypes.Target).Descriptor.IsStarVicinity),
            New EnterInteriorInteractionTypeDescriptor(EnterOrbit, "Enter Orbit", Function(x) x.Descriptor.IsSatellite OrElse x.Descriptor.IsPlanet),
            New EnterInteriorInteractionTypeDescriptor(Approach, "Approach", Function(x) x.Descriptor.IsPlanetVicinity OrElse x.Descriptor.IsStarSystem OrElse x.Descriptor.IsStarVicinity),
            New TradeInteractionTypeDescriptor(),
            New ShipyardInteractionTypeDescriptor(),
            New UseFuelScoopInterationTypeDescriptor(),
            New UseAirScoopInteractionTypeDescriptor(),
            New AcceptDeliveryMissionInteractionTypeDescriptor()
        }.ToDictionary(Function(x) x.InteractionType, Function(x) x)
End Module
