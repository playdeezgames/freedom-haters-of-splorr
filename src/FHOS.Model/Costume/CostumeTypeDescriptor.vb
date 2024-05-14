Friend Class CostumeTypeDescriptor
    ReadOnly Property Name As String
    ReadOnly Property Glyphs As Char()
    ReadOnly Property Hue As Integer
    Sub New(name As String, glyphs As Char(), hue As Integer)
        Me.Name = name
        Me.Glyphs = glyphs
        Me.Hue = hue
    End Sub
End Class
