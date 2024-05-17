Friend Class LocationTypeDescriptor
    Sub New(
           name As String,
           glyph As Char,
           foreground As Integer,
           background As Integer,
           Optional canPlaceWormhole As Boolean = False,
           Optional isEnterable As Boolean = True)
        Me.Name = name
        Me.Glyph = glyph
        Me.Foreground = foreground
        Me.Background = background
        Me.CanPlaceWormhole = canPlaceWormhole
        Me.IsEnterable = isEnterable
    End Sub
    ReadOnly Property Name As String
    ReadOnly Property Glyph As Char
    ReadOnly Property Background As Integer
    ReadOnly Property Foreground As Integer
    ReadOnly Property CanPlaceWormhole As Boolean
    ReadOnly Property IsEnterable As Boolean
End Class
