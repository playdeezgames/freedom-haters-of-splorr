Friend Class CellModel
    Implements ICellModel

    Private ReadOnly cell As Persistence.ICell

    Public Sub New(world As Persistence.IWorld, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (boardPosition.X + world.Avatar.Cell.Column, boardPosition.Y + world.Avatar.Cell.Row)
        Me.cell = world.Avatar.Cell.Map.GetCell(mapPosition.X, mapPosition.Y)
    End Sub

    Public ReadOnly Property Exists As Boolean Implements ICellModel.Exists
        Get
            Return cell IsNot Nothing
        End Get
    End Property
End Class
