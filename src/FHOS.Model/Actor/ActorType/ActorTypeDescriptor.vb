Imports FHOS.Persistence

Friend Class ActorTypeDescriptor
    ReadOnly Property Glyphs As Char()
    ReadOnly Property Hue As Integer
    ReadOnly Property MaximumOxygen As Integer
    ReadOnly Property MaximumFuel As Integer
    ReadOnly Property CanSpawn As Func(Of ILocation, Boolean)
    Sub New(
           glyphs As Char(),
           hue As Integer,
           Optional maximumOxygen As Integer = 0,
           Optional maximumFuel As Integer = 0,
           Optional canSpawn As Func(Of ILocation, Boolean) = Nothing)
        Me.Glyphs = glyphs
        Me.Hue = hue
        Me.MaximumOxygen = maximumOxygen
        Me.MaximumFuel = maximumFuel
        If canSpawn Is Nothing Then
            canSpawn = Function(x) True
        End If
        Me.CanSpawn = canSpawn
    End Sub
End Class
