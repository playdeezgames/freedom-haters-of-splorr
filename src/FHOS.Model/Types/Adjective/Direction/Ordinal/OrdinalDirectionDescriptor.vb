Friend Class OrdinalDirectionDescriptor
    ReadOnly Property DirectionName As String
    ReadOnly Property Glyph As Char

    Public Sub New(directionName As String, glyph As Char)
        Me.DirectionName = directionName
        Me.Glyph = glyph
    End Sub
End Class
