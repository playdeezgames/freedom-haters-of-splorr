Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        New List(Of ActorTypeDescriptor) From
        {
            New PlayerShipDescriptor(),
            New MilitaryVesselDescriptor(),
            New PersonDescriptor()
        }.
        ToDictionary(Function(x) x.ActorType, Function(x) x)
End Module
