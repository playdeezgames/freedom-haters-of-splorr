Imports SPLORR.Game

Friend Class GalacticAgeDescriptor
    ReadOnly Property Ordinal As Integer
    ReadOnly Property DisplayName As String
    ReadOnly Property StarTypeGenerator As IReadOnlyDictionary(Of String, Integer)
    Sub New(displayName As String, ordinal As Integer, starTypeGenerator As IReadOnlyDictionary(Of String, Integer))
        Me.DisplayName = displayName
        Me.Ordinal = ordinal
        Me.StarTypeGenerator = starTypeGenerator
    End Sub
    Function GenerateStarType() As String
        Return RNG.FromGenerator(StarTypeGenerator)
    End Function
End Class
