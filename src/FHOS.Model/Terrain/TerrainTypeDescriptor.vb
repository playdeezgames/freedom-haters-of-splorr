Friend Class TerrainTypeDescriptor
    Sub New(name As String, glyph As Char, foreground As Integer, background As Integer)
        Me.Name = name
        Me.Glyph = glyph
        Me.Foreground = foreground
        Me.Background = background
    End Sub
    ReadOnly Property Name As String
    ReadOnly Property Glyph As Char
    ReadOnly Property Background As Integer
    ReadOnly Property Foreground As Integer

End Class
