Friend Module GroupTypes
    Friend ReadOnly Faction As String = NameOf(Faction)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, GroupTypeDescriptor) =
        New List(Of GroupTypeDescriptor) From
        {
            New FactionGroupTypeDescriptor(),
            New StarSystemGroupTypeDescriptor()
        }.ToDictionary(Function(x) x.GroupType, Function(x) x)
End Module
