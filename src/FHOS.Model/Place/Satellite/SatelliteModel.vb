Friend Class SatelliteModel
    Inherits PlaceModel
    Implements ISatelliteModel

    Private ReadOnly satellite As Persistence.ISatellite

    Public Sub New(satellite As Persistence.ISatellite)
        MyBase.New(satellite)
        Me.satellite = satellite
    End Sub
End Class
