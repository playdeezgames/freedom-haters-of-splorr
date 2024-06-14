Friend Module FacingTypes
    Friend Const Up = 0
    Friend Const Right = 1
    Friend Const Down = 2
    Friend Const Left = 3
    Friend ReadOnly Descriptors As Dictionary(Of Integer, FacingDescriptor) =
        New List(Of FacingDescriptor) From
        {
            New FacingDescriptor(Up, (0, -1)),
            New FacingDescriptor(Right, (1, 0)),
            New FacingDescriptor(Down, (0, 1)),
            New FacingDescriptor(Left, (-1, 0))
        }.ToDictionary(Function(x) x.Facing, Function(x) x)
    Friend ReadOnly All As IEnumerable(Of Integer) = Descriptors.Keys
    Friend ReadOnly Deltas As (X As Integer, Y As Integer)() =
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        }
End Module
