Imports FHOS.Persistence

Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly avatar As ICharacter

    Public Sub New(avatar As ICharacter)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return avatar.Cell.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
        Get
            Return avatar.Cell.Row
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarModel.MapName
        Get
            Return avatar.Cell.Map.Name
        End Get
    End Property

    Public ReadOnly Property OxygenHue As Integer Implements IAvatarModel.OxygenHue
        Get
            If OxygenPercent <= 33 Then
                Return Hue.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.Oxygen * 100 \ avatar.MaximumOxygen
        End Get
    End Property

    Public Sub Move(delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        Dim cell = avatar.Cell
        Dim nextColumn = cell.Column + delta.X
        Dim nextRow = cell.Row + delta.Y
        Dim map = cell.Map
        Dim nextCell = map.GetCell(nextColumn, nextRow)
        If nextCell IsNot Nothing Then
            avatar.Cell = nextCell
        End If
    End Sub

    Public Sub SetFacing(facing As Integer) Implements IAvatarModel.SetFacing
        avatar.Facing = facing
    End Sub
End Class
