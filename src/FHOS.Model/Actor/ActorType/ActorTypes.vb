Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)
    Friend ReadOnly TradingPost As String = NameOf(TradingPost)
    Friend ReadOnly StarDock As String = NameOf(StarDock)
    Friend ReadOnly Shipyard As String = NameOf(Shipyard)
    Friend ReadOnly StarGate As String = NameOf(StarGate)
    Friend ReadOnly Debris As String = NameOf(Debris)
    Friend ReadOnly Wormhole As String = NameOf(Wormhole)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New List(Of ActorTypeDescriptor) From
        {
            New PlayerShipActorTypeDescriptor(),
            New MilitaryVesselActorTypeDescriptor(),
            New PersonActorTypeDescriptor(),
            New TradingPostActorTypeDescriptor(),
            New StarDockActorTypeDescriptor(),
            New ShipyardActorTypeDescriptor(),
            New StarGateActorTypeDescriptor(),
            New DebrisActorTypeDescriptor(),
            New WormholeActorTypeDescriptor()
        }.
        ToDictionary(Function(x) x.ActorType, Function(x) x)
End Module
