﻿Friend Class Grid5x5Descriptor
    Friend ReadOnly Property SectionName As String
    Friend ReadOnly Property Glyph As Char

    Public Sub New(sectionName As String, glyph As Char)
        Me.SectionName = sectionName
        Me.Glyph = glyph
    End Sub
End Class