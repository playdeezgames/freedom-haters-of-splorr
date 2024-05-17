Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)
    Friend ReadOnly TradingPost As String = NameOf(TradingPost)
    Friend ReadOnly StarDock As String = NameOf(StarDock)
    Friend ReadOnly Shipyard As String = NameOf(Shipyard)
    Friend ReadOnly StarGate As String = NameOf(StarGate)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New List(Of ActorTypeDescriptor) From
        {
            New PlayerShipDescriptor(),
            New MilitaryVesselDescriptor(),
            New PersonDescriptor(),
            New TradingPostDescriptor(),
            New StarDockDescriptor(),
            New ShipyardDescriptor(),
            New StarGateDescriptor()
        }.
        ToDictionary(Function(x) x.ActorType, Function(x) x)
End Module
