Imports FHOS.Persistence

Friend Class CharacterTypeDescriptor
    ReadOnly Property Glyph As Char
    ReadOnly Property Hue As Integer
    ReadOnly Property MaximumOxygen As Integer
    ReadOnly Property CanSpawn As Func(Of ICell, Boolean)
    Sub New(glyph As Char, hue As Integer, maximumOxygen As Integer, Optional canSpawn As Func(Of ICell, Boolean) = Nothing)
        Me.Glyph = glyph
        Me.Hue = hue
        Me.MaximumOxygen = maximumOxygen
        If canSpawn Is Nothing Then
            canSpawn = Function(x) True
        End If
        Me.CanSpawn = canSpawn
    End Sub
End Class
