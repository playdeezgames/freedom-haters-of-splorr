Friend Class OrdinalDirectionDescriptor
    ReadOnly Property DirectionName As String
    ReadOnly Property Glyph As Char
    Friend ReadOnly Property Delta As (X As Integer, Y As Integer)

    Public Sub New(
                  directionName As String,
                  glyph As Char,
                  delta As (X As Integer, Y As Integer))
        Me.DirectionName = directionName
        Me.Glyph = glyph
        Me.Delta = delta
    End Sub
End Class
