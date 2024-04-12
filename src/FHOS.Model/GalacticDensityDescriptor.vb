Friend Class GalacticDensityDescriptor
    ReadOnly Property Ordinal As Integer
    ReadOnly Property DisplayName As String
    ReadOnly Property MinimumDistance As Integer
    Sub New(displayName As String, ordinal As Integer, minimumDistance As Integer)
        Me.DisplayName = displayName
        Me.Ordinal = ordinal
        Me.MinimumDistance = minimumDistance
    End Sub
End Class
