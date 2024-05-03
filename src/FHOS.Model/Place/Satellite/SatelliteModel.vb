Friend Class SatelliteModel
    Inherits PlaceModel
    Implements ISatelliteModel

    Private ReadOnly satellite As Persistence.IPlace

    Public Sub New(satellite As Persistence.IPlace)
        MyBase.New(satellite)
        Me.satellite = satellite
    End Sub
End Class
