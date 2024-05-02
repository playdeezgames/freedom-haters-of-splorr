Friend Class SatelliteModel
    Implements ISatelliteModel

    Private ReadOnly satellite As Persistence.ISatellite

    Public Sub New(satellite As Persistence.ISatellite)
        Me.satellite = satellite
    End Sub

    Public ReadOnly Property Name As String Implements ISatelliteModel.Name
        Get
            Return satellite.Name
        End Get
    End Property
End Class
