Friend Module GroupTypes
    Friend ReadOnly Faction As String = NameOf(Faction)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, GroupTypeDescriptor) =
        New List(Of GroupTypeDescriptor) From
        {
            New FactionGroupTypeDescriptor()
        }.ToDictionary(Function(x) x.GroupType, Function(x) x)
End Module
