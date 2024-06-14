Public Interface ILocationTypeModel
    ReadOnly Property Glyph As Char 'stuff down, combine with Foreground, Background
    ReadOnly Property Foreground As Integer 'stuff down, combine with Glyph, Background
    ReadOnly Property Background As Integer 'stuff down, combine with Glyph, Foreground
    ReadOnly Property Name As String 'stuff down
End Interface
