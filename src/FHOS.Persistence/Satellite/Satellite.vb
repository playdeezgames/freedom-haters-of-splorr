﻿Friend Class Satellite
    Inherits SatelliteDataClient
    Implements ISatellite

    Public Sub New(universeData As Data.UniverseData, satelliteId As Integer)
        MyBase.New(universeData, satelliteId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ISatellite.Id
        Get
            Return SatelliteId
        End Get
    End Property
End Class