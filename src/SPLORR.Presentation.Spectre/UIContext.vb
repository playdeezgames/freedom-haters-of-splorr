Imports System.Text
Imports Spectre.Console

Public Class UIContext
    Implements IUIContext
    Private needsClear As Boolean = False
    Private ReadOnly frameBuffer As New StringBuilder

    Public Sub Clear() Implements IUIContext.Clear
        needsClear = True
        frameBuffer.Clear()
    End Sub

    Public Sub Message(prompt As (Mood As Mood, Text As String)) Implements IUIContext.Message
        Choose(prompt, "Ok")
    End Sub

    Public Sub WriteLine(ParamArray lines() As (Mood As Mood, Text As String)) Implements IUIContext.WriteLine
        For Each line In lines
            frameBuffer.AppendLine($"[{line.Mood.ColorName}]{Markup.Escape(line.Text)}[/]")
        Next
    End Sub

    Public Sub WriteExceptionImmediate(ex As Exception) Implements IUIContext.WriteExceptionImmediate
        AnsiConsole.WriteException(ex)
    End Sub

    Public Sub WriteFigletImmediate(figlet As (Mood As Mood, Text As String)) Implements IUIContext.WriteFigletImmediate
        AnsiConsole.Write(New FigletText(figlet.Text) With
            {
                .Color = figlet.Mood.ToColor()
            })
    End Sub

    Public Sub Write(stuff As (Mood As Mood, Text As String)) Implements IUIContext.Write
        frameBuffer.Append($"[{stuff.Mood.ColorName}]{Markup.Escape(stuff.Text)}[/]")
    End Sub

    Public Function Choose(Of TResult)(prompt As (Mood As Mood, Text As String), ParamArray choices() As (Text As String, Value As TResult)) As TResult Implements IUIContext.Choose
        UpdateFrame()
        Dim table = choices.ToDictionary(Function(x) x.Text, Function(x) x.Value)
        Dim selector As New SelectionPrompt(Of String) With {.Title = $"[{prompt.Mood.ColorName}]{Markup.Escape(prompt.Text)}[/]"}
        selector.AddChoices(choices.Select(Function(x) Markup.Escape(x.Text)))
        Return table(AnsiConsole.Prompt(selector))
    End Function

    Private Sub UpdateFrame()
        If needsClear Then
            AnsiConsole.Clear()
            needsClear = False
        End If
        AnsiConsole.Markup(frameBuffer.ToString)
        frameBuffer.Clear()
    End Sub

    Public Function Choose(prompt As (Mood As Mood, Text As String), ParamArray choices() As String) As String Implements IUIContext.Choose
        Return Choose(Of String)(prompt, choices.Select(Function(x) (x, x)).ToArray)
    End Function

    Public Function Confirm(prompt As (Mood As Mood, Text As String)) As Boolean Implements IUIContext.Confirm
        Const Yes = "Yes"
        Const No = "No"
        Return Choose(prompt, No, Yes) = Yes
    End Function

    Public Function Ask(Of TResult)(prompt As (Mood As Mood, Text As String), defaultResult As TResult) As TResult Implements IUIContext.Ask
        UpdateFrame()
        Return AnsiConsole.Ask($"[{prompt.Mood.ColorName}]{prompt.Text}[/]", defaultResult)
    End Function

    Public Function ReadKey() As String Implements IUIContext.ReadKey
        UpdateFrame()
        Do
            Dim key = AnsiConsole.Console.Input.ReadKey(True)
            If key.HasValue Then
                Return key?.Key.ToString()
            End If
        Loop
    End Function

    Public Sub ClearImmediate() Implements IUIContext.ClearImmediate
        AnsiConsole.Clear()
    End Sub

    Public Sub WriteLineImmediate(ParamArray lines() As (Mood As Mood, Text As String)) Implements IUIContext.WriteLineImmediate
        For Each line In lines
            AnsiConsole.MarkupLine($"[{line.Mood.ColorName}]{Markup.Escape(line.Text)}[/]")
        Next
    End Sub
End Class
