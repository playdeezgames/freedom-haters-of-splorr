Friend Module Facings
    Friend Const Up = 0
    Friend Const Right = 1
    Friend Const Down = 2
    Friend Const Left = 3
    Friend ReadOnly All As IEnumerable(Of Integer) = {Up, Right, Down, Left}
    Friend ReadOnly Deltas As (X As Integer, Y As Integer)() =
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        }
End Module
