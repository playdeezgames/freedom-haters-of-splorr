Module OrdinalDirections
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly NorthEast As String = NameOf(NorthEast)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly SouthEast As String = NameOf(SouthEast)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly SouthWest As String = NameOf(SouthWest)
    Friend ReadOnly West As String = NameOf(West)
    Friend ReadOnly NorthWest As String = NameOf(NorthWest)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, OrdinalDirectionDescriptor) =
        New List(Of OrdinalDirectionDescriptor) From
        {
            New OrdinalDirectionDescriptor(North, ChrW(16), (0, -1)),
            New OrdinalDirectionDescriptor(NorthEast, ChrW(17), (1, -1)),
            New OrdinalDirectionDescriptor(East, ChrW(18), (1, 0)),
            New OrdinalDirectionDescriptor(SouthEast, ChrW(19), (1, 1)),
            New OrdinalDirectionDescriptor(South, ChrW(20), (0, 1)),
            New OrdinalDirectionDescriptor(SouthWest, ChrW(21), (-1, 1)),
            New OrdinalDirectionDescriptor(West, ChrW(22), (-1, 0)),
            New OrdinalDirectionDescriptor(NorthWest, ChrW(23), (-1, -1))
        }.ToDictionary(Function(x) x.DirectionName, Function(x) x)

End Module
