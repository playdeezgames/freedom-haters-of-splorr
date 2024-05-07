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
    Sub New(satelliteType As String)
        Me.SatelliteType = satelliteType
    End Sub
End Class
