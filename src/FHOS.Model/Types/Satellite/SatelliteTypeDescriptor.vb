Friend Class SatelliteTypeDescriptor
    ReadOnly Property SatelliteType As String
    ReadOnly Property Hue As Integer
    Sub New(satelliteType As String, hue As Integer)
        Me.SatelliteType = satelliteType
        Me.Hue = hue
    End Sub
End Class
