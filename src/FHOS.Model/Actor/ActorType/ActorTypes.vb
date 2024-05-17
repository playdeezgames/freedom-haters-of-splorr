Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)
    Friend ReadOnly TradingPost As String = NameOf(TradingPost)
    'Friend ReadOnly SpaceDock As String = NameOf(SpaceDock)
    'Friend ReadOnly Shipyard As String = NameOf(Shipyard)
    'Friend ReadOnly Stargate As String = NameOf(Stargate)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New List(Of ActorTypeDescriptor) From
        {
            New PlayerShipDescriptor(),
            New MilitaryVesselDescriptor(),
            New PersonDescriptor(),
            New TradingPostDescriptor()
        }.
        ToDictionary(Function(x) x.ActorType, Function(x) x)
End Module
