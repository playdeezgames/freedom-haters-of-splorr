Imports SPLORR.Game

Friend Class StarTypeDescriptor
    Sub New(
           starTypeName As String,
           locationType As String,
           generatorWeight As Integer,
           minimumPlanetaryDistance As Integer,
           planetTypeTable As IReadOnlyDictionary(Of String, Integer))
        Me.StarTypeName = starTypeName
        Me.LocationType = locationType
        Me.GeneratorWeight = generatorWeight
        Me.MinimumPlanetaryDistance = minimumPlanetaryDistance
        Me.planetTypeTable = planetTypeTable
    End Sub
    Friend ReadOnly Property StarTypeName As String
    Friend ReadOnly Property LocationType As String
    Friend ReadOnly Property GeneratorWeight As Integer
    Friend ReadOnly Property MinimumPlanetaryDistance As Integer
    Private ReadOnly planetTypeTable As IReadOnlyDictionary(Of String, Integer)
    Friend Function GeneratePlanetType() As String
        Return RNG.FromGenerator(planetTypeTable)
    End Function
End Class
