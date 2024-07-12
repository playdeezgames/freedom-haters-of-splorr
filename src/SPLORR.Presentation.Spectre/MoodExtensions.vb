Imports System.Runtime.CompilerServices
Imports Spectre.Console

Friend Module MoodExtensions
    <Extension>
    Friend Function ColorName(mood As Mood) As String
        Select Case mood
            Case Mood.Prompt
                Return "olive"
            Case Mood.Danger
                Return "red"
            Case Mood.Info
                Return "silver"
            Case Mood.Success
                Return "lime"
            Case Mood.Title
                Return "fuchsia"
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    <Extension>
    Friend Function ToColor(mood As Mood) As Color
        Select Case mood
            Case Mood.Prompt
                Return Color.Olive
            Case Mood.Danger
                Return Color.Red
            Case Mood.Info
                Return Color.Silver
            Case Mood.Success
                Return Color.Lime
            Case Mood.Title
                Return Color.Fuchsia
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Module
