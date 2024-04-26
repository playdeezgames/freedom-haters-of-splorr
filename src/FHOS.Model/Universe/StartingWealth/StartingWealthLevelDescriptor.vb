Imports SPLORR.Game

Friend Class StartingWealthLevelDescriptor
    ReadOnly Property Ordinal As Integer
    ReadOnly Property DisplayName As String
    ReadOnly Property JoolsGenerator As IReadOnlyDictionary(Of Integer, Integer)
    Sub New(displayName As String, ordinal As Integer, joolsGenerator As IReadOnlyDictionary(Of Integer, Integer))
        Me.DisplayName = displayName
        Me.Ordinal = ordinal
        Me.JoolsGenerator = joolsGenerator
    End Sub
    Function GenerateJools() As Integer
        Return RNG.FromGenerator(JoolsGenerator)
    End Function

End Class
