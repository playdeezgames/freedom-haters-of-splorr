Friend Module CardinalDirections
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly West As String = NameOf(West)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CardinalDirectionDescriptor) =
        New List(Of CardinalDirectionDescriptor) From
        {
            New CardinalDirectionDescriptor(North),
            New CardinalDirectionDescriptor(East),
            New CardinalDirectionDescriptor(South),
            New CardinalDirectionDescriptor(West)
        }.ToDictionary(Function(x) x.DirectionName, Function(x) x)
End Module
