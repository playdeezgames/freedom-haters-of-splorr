Imports FHOS.Data

Friend Class SatelliteDataClient
    Inherits UniverseDataClient

    Public Sub New(universeData As Data.UniverseData, satelliteId As Integer)
        MyBase.New(universeData)
        Me.SatelliteId = satelliteId
    End Sub

    Protected ReadOnly Property SatelliteId As Integer
    Protected ReadOnly Property SatelliteData As SatelliteData
End Class
