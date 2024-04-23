Imports SPLORR.Game

Friend Class PlanetTypeDescriptor
    Friend ReadOnly Property LocationType As String
    Friend ReadOnly Property MinimumSatelliteDistance As Integer

    Sub New(
           locationType As String,
           minimumSatelliteDistance As Integer,
           satelliteTypeTable As IReadOnlyDictionary(Of String, Integer))
        Me.LocationType = locationType
        Me.MinimumSatelliteDistance = minimumSatelliteDistance
        Me.satelliteTypeTable = satelliteTypeTable
    End Sub
    Private ReadOnly satelliteTypeTable As IReadOnlyDictionary(Of String, Integer)

    Friend Function GenerateSatelliteType() As String
        Return RNG.FromGenerator(satelliteTypeTable)
    End Function
End Class
