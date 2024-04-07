Friend Class CellModel
    Implements ICellModel

    Private world As Persistence.IWorld
    Private boardPosition As (X As Integer, Y As Integer)

    Public Sub New(world As Persistence.IWorld, boardPosition As (X As Integer, Y As Integer))
        Me.world = world
        Me.boardPosition = boardPosition
    End Sub
End Class
