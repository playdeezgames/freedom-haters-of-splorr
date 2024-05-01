Imports SPLORR.Game

Friend Class PlanetTypeDescriptor
    Friend ReadOnly Property LocationType As String
    Friend ReadOnly Property MinimumSatelliteDistance As Integer
    Friend ReadOnly Property CanRefillOxygen As Boolean

    Sub New(
           locationType As String,
           minimumSatelliteDistance As Integer,
           satelliteTypeTable As IReadOnlyDictionary(Of String, Integer),
           Optional canRefillOxygen As Boolean = False)
        Me.LocationType = locationType
        Me.MinimumSatelliteDistance = minimumSatelliteDistance
        Me.satelliteTypeTable = satelliteTypeTable
        Me.CanRefillOxygen = canRefillOxygen
    End Sub
    Private ReadOnly satelliteTypeTable As IReadOnlyDictionary(Of String, Integer)

    Friend Function GenerateSatelliteType() As String
        Return RNG.FromGenerator(satelliteTypeTable)
    End Function
End Class
