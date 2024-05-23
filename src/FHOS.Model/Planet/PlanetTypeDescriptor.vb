Imports SPLORR.Game

Friend Class PlanetTypeDescriptor
    Friend ReadOnly Property LocationType As String
        Get
            Return LocationTypes.MakePlanetLocationType(PlanetType)
        End Get
    End Property
    Friend Function SectionLocationType(sectionName As String) As String
        Return LocationTypes.MakePlanetSectionLocationType(PlanetType, sectionName)
    End Function
    Friend ReadOnly Property MinimumSatelliteDistance As Integer
    Friend ReadOnly Property CanRefillOxygen As Boolean
    Friend ReadOnly Property PlanetType As String
    Friend ReadOnly Property Hue As Integer

    Sub New(
           planetType As String,
           hue As Integer,
           minimumSatelliteDistance As Integer,
           satelliteTypeTable As IReadOnlyDictionary(Of String, Integer),
           satelliteCountTable As IReadOnlyDictionary(Of Integer, Integer),
           Optional canRefillOxygen As Boolean = False)
        Me.Hue = hue
        Me.PlanetType = planetType
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
