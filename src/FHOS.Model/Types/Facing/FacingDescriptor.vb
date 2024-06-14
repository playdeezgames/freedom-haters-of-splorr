Friend Class FacingDescriptor
    Friend ReadOnly Facing As Integer
    Friend ReadOnly Delta As (X As Integer, Y As Integer)

    Public Sub New(up As Integer, value As (X As Integer, Y As Integer))
        Me.facing = up
        Me.delta = value
    End Sub
End Class
