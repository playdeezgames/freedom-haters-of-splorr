Public Module Facing
    Public Const Up = 0
    Public Const Right = 1
    Public Const Down = 2
    Public Const Left = 3
    Public ReadOnly Deltas As (X As Integer, Y As Integer)() =
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        }
End Module
