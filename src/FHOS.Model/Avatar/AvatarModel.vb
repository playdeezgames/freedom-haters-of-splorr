Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As Persistence.IWorld

    Public Sub New(world As Persistence.IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return world.Avatar.Cell.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
        Get
            Return world.Avatar.Cell.Row
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarModel.MapName
        Get
            Return world.Avatar.Cell.Map.Name
        End Get
    End Property

    Public Sub Move(delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        Dim cell = world.Avatar.Cell
        Dim nextColumn = cell.Column + delta.X
        Dim nextRow = cell.Row + delta.Y
        Dim map = cell.Map
        Dim nextCell = map.GetCell(nextColumn, nextRow)
        If nextCell IsNot Nothing Then
            world.Avatar.Cell = nextCell
        End If
    End Sub
End Class
