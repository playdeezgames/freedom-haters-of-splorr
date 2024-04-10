Friend Class TerrainDescriptor
    Sub New(glyph As Char, foreground As Integer, background As Integer)
        Me.Glyph = glyph
        Me.Foreground = foreground
        Me.Background = background
    End Sub
    ReadOnly Property Glyph As Char
    ReadOnly Property Background As Integer
    ReadOnly Property Foreground As Integer

End Class
