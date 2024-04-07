Friend Class BoardModel
    Implements IBoardModel

    Private ReadOnly world As Persistence.IWorld

    Public Sub New(world As Persistence.IWorld)
        Me.world = world
    End Sub

    Public Function GetCell(boardPosition As (X As Integer, Y As Integer)) As ICellModel Implements IBoardModel.GetCell
        Return New CellModel(world, boardPosition)
    End Function
End Class
