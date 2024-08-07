Friend Module CardinalDirections
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly West As String = NameOf(West)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, CardinalDirectionDescriptor) =
        New List(Of CardinalDirectionDescriptor) From
        {
            New CardinalDirectionDescriptor(North, (0, -1)),
            New CardinalDirectionDescriptor(East, (1, 0)),
            New CardinalDirectionDescriptor(South, (0, 1)),
            New CardinalDirectionDescriptor(West, (-1, 0))
        }.ToDictionary(Function(x) x.DirectionName, Function(x) x)
End Module
