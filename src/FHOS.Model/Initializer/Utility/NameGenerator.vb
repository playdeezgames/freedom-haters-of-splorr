Imports System.Text
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class NameGenerator
    Private ReadOnly usedNames As New HashSet(Of String)


    Public Function GenerateUnusedName() As String
        Dim name As String
        Do
            name = GenerateName()
        Loop While usedNames.Contains(name)
        usedNames.Add(name)
        Return name
    End Function

    Private ReadOnly vowelTable As IReadOnlyDictionary(Of String, Integer) = New Dictionary(Of String, Integer) From
        {
            {"a", 1},
            {"e", 1},
            {"i", 1},
            {"o", 1},
            {"u", 1}
        }
    Private ReadOnly consonantTable As IReadOnlyDictionary(Of String, Integer) = New Dictionary(Of String, Integer) From
        {
            {"b", 1},
            {"ch", 1},
            {"d", 1},
            {"f", 1},
            {"g", 1},
            {"k", 1},
            {"l", 1},
            {"m", 1},
            {"n", 1},
            {"p", 1},
            {"r", 1},
            {"s", 1},
            {"sh", 1},
            {"t", 1},
            {"th", 1},
            {"v", 1},
            {"z", 1},
            {"zh", 1}
        }
    Private Function GenerateName() As String
        Dim builder As New StringBuilder
        Dim isVowel = RNG.FromEnumerable({False, True})
        For Each index In Enumerable.Range(0, RNG.RollDice("3d4"))
            If isVowel Then
                builder.Append(RNG.FromGenerator(vowelTable))
            Else
                builder.Append(RNG.FromGenerator(consonantTable))
            End If
            isVowel = Not isVowel
        Next
        Dim name = builder.ToString
        Return name.Substring(0, 1).ToUpper + name.Substring(1)
    End Function
End Class
