Friend Class SatelliteTypeDescriptor
    Friend Function SectionLocationType(sectionName As String) As String
        Return LocationTypes.MakeSatelliteSectionLocationType(SatelliteType, sectionName)
    End Function
    ReadOnly Property LocationType As String
        Get
            Return LocationTypes.MakeSatelliteLocationType(SatelliteType)
        End Get
    End Property
    ReadOnly Property SatelliteType As String
    ReadOnly Property Hue As Integer
    Sub New(satelliteType As String, hue As Integer)
        Me.SatelliteType = satelliteType
        Me.Hue = hue
    End Sub
End Class
