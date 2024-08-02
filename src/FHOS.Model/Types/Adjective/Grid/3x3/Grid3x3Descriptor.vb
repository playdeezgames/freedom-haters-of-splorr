Friend Class Grid3x3Descriptor
    Friend ReadOnly Property SectionName As String
    Friend ReadOnly Property Glyph As Char
    Friend ReadOnly Property Delta As (X As Integer, Y As Integer)

    Public Sub New(sectionName As String, glyph As Char, delta As (X As Integer, Y As Integer))
        Me.SectionName = sectionName
        Me.Glyph = glyph
        Me.Delta = delta
    End Sub
End Class
