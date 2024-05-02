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

    Const vowels = "aeiou"
    Const consonants = "bdfghklmnprstv"
    Private Function GenerateName() As String
        Dim builder As New StringBuilder
        Dim isVowel = RNG.FromEnumerable({False, True})
        For Each index In Enumerable.Range(0, RNG.RollDice("4d3"))
            If isVowel Then
                builder.Append(RNG.FromEnumerable(vowels))
            Else
                builder.Append(RNG.FromEnumerable(consonants))
            End If
            isVowel = Not isVowel
        Next
        Dim name = builder.ToString
        Return name.Substring(0, 1).ToUpper + name.Substring(1)
    End Function
End Class
