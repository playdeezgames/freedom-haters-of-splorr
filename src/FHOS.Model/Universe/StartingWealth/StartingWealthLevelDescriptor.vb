Imports SPLORR.Game

Friend Class StartingWealthLevelDescriptor
    ReadOnly Property Ordinal As Integer
    ReadOnly Property DisplayName As String
    ReadOnly Property JoolsGenerator As IReadOnlyDictionary(Of Integer, Integer)
    ReadOnly Property MinimumJools As Integer
    Sub New(
           displayName As String,
           ordinal As Integer,
           joolsGenerator As IReadOnlyDictionary(Of Integer, Integer),
           Optional minimumJools As Integer = 0)
        Me.DisplayName = displayName
        Me.Ordinal = ordinal
        Me.JoolsGenerator = joolsGenerator
        Me.MinimumJools = minimumJools
    End Sub
    Function GenerateJools() As Integer
        Return RNG.FromGenerator(JoolsGenerator)
    End Function
End Class
