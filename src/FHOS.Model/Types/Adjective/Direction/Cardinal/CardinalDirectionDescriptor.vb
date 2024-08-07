Friend Class CardinalDirectionDescriptor
    Friend ReadOnly Property DirectionName As String
    Friend ReadOnly Property Delta As (X As Integer, Y As Integer)

    Public Sub New(
                  directionName As String,
                  delta As (X As Integer, Y As Integer))
        Me.DirectionName = directionName
        Me.Delta = delta
    End Sub
End Class
