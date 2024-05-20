Module OrdinalDirections
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly NorthEast As String = NameOf(NorthEast)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly SouthEast As String = NameOf(SouthEast)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly SouthWest As String = NameOf(SouthWest)
    Friend ReadOnly West As String = NameOf(West)
    Friend ReadOnly NorthWest As String = NameOf(NorthWest)

    Friend ReadOnly All As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        }

End Module
