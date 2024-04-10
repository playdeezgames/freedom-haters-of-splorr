Friend Class CharacterTypeDescriptor
    ReadOnly Property Glyph As Char
    ReadOnly Property Hue As Integer
    Sub New(glyph As Char, hue As Integer)
        Me.Glyph = glyph
        Me.Hue = hue
    End Sub
End Class
