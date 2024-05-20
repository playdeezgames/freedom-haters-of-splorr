Friend Module CardinalDirections
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly West As String = NameOf(West)

    Friend ReadOnly All As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            North,
            East,
            South,
            West
        }
End Module
