Imports SPLORR.Game

Friend Class PlanetTypeDescriptor
    Friend ReadOnly Property LocationType As String
    Friend ReadOnly Property MinimumSatelliteDistance As Integer
    Friend ReadOnly Property CanRefillOxygen As Boolean

    Sub New(
           planetType As String,
           minimumSatelliteDistance As Integer,
           satelliteTypeTable As IReadOnlyDictionary(Of String, Integer),
           satelliteCountTable As IReadOnlyDictionary(Of Integer, Integer),
           Optional canRefillOxygen As Boolean = False)
        Me.LocationType = LocationTypes.MakePlanetLocationType(planetType)
        Me.MinimumSatelliteDistance = minimumSatelliteDistance
        Me.satelliteTypeTable = satelliteTypeTable
        Me.CanRefillOxygen = canRefillOxygen
        Me.satelliteCountTable = satelliteCountTable
    End Sub
    Private ReadOnly satelliteTypeTable As IReadOnlyDictionary(Of String, Integer)
    Private ReadOnly satelliteCountTable As IReadOnlyDictionary(Of Integer, Integer)
    Friend Function GenerateSatelliteType() As String
        Return RNG.FromGenerator(satelliteTypeTable)
    End Function

    Friend Function GenerateMaximumSatelliteCount() As Integer
        Return RNG.FromGenerator(satelliteCountTable)
    End Function
End Class
