Imports SPLORR.Game

Friend Class StarTypeDescriptor
    Sub New(
           starTypeName As String,
           hue As Integer,
           generatorWeight As Integer,
           minimumPlanetaryDistance As Integer,
           planetTypeTable As IReadOnlyDictionary(Of String, Integer),
           planetCountTable As IReadOnlyDictionary(Of Integer, Integer))
        Me.StarTypeName = starTypeName
        Me.Hue = hue
        Me.GeneratorWeight = generatorWeight
        Me.MinimumPlanetaryDistance = minimumPlanetaryDistance
        Me.planetTypeTable = planetTypeTable
        Me.planetCountTable = planetCountTable
    End Sub
    Friend ReadOnly Property StarTypeName As String
    Friend ReadOnly Property Hue As Integer
    Friend ReadOnly Property GeneratorWeight As Integer
    Friend ReadOnly Property MinimumPlanetaryDistance As Integer
    Private ReadOnly planetTypeTable As IReadOnlyDictionary(Of String, Integer)
    Private ReadOnly planetCountTable As IReadOnlyDictionary(Of Integer, Integer)
    Friend Function GeneratePlanetType() As String
        Return RNG.FromGenerator(planetTypeTable)
    End Function

    Friend Function GenerateMaximumPlanetCount() As Integer
        Return RNG.FromGenerator(planetCountTable)
    End Function
End Class
