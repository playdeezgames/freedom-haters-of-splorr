Imports Spectre.Console

Public Class UIContext
    Implements IUIContext

    Public Sub Clear() Implements IUIContext.Clear
        AnsiConsole.Clear()
    End Sub

    Public Sub Message(prompt As (Mood As Mood, Text As String)) Implements IUIContext.Message
        Choose(prompt, "Ok")
    End Sub

    Public Sub WriteLine(ParamArray lines() As (Mood As Mood, Text As String)) Implements IUIContext.WriteLine
        For Each line In lines
            AnsiConsole.MarkupLine($"[{line.Mood.ColorName}]{Markup.Escape(line.Text)}[/]")
        Next
    End Sub

    Public Sub WriteException(ex As Exception) Implements IUIContext.WriteException
        AnsiConsole.WriteException(ex)
    End Sub

    Public Sub WriteFiglet(figlet As (Mood As Mood, Text As String)) Implements IUIContext.WriteFiglet
        AnsiConsole.Write(New FigletText(figlet.Text) With
            {
                .Color = figlet.Mood.ToColor()
            })
    End Sub

    Public Sub Write(stuff As (Mood As Mood, Text As String)) Implements IUIContext.Write
        AnsiConsole.Markup($"[{stuff.Mood.ColorName}]{Markup.Escape(stuff.Text)}[/]")
    End Sub

    Public Function Choose(Of TResult)(prompt As (Mood As Mood, Text As String), ParamArray choices() As (Text As String, Value As TResult)) As TResult Implements IUIContext.Choose
        Dim table = choices.ToDictionary(Function(x) x.Text, Function(x) x.Value)
        Dim selector As New SelectionPrompt(Of String) With {.Title = $"[{prompt.Mood.ColorName}]{Markup.Escape(prompt.Text)}[/]"}
        selector.AddChoices(choices.Select(Function(x) Markup.Escape(x.Text)))
        Return table(AnsiConsole.Prompt(selector))
    End Function

    Public Function Choose(prompt As (Mood As Mood, Text As String), ParamArray choices() As String) As String Implements IUIContext.Choose
        Return Choose(Of String)(prompt, choices.Select(Function(x) (x, x)).ToArray)
    End Function

    Public Function Confirm(prompt As (Mood As Mood, Text As String)) As Boolean Implements IUIContext.Confirm
        Const Yes = "Yes"
        Const No = "No"
        Return Choose(prompt, No, Yes) = Yes
    End Function

    Public Function Ask(Of TResult)(prompt As (Mood As Mood, Text As String), defaultResult As TResult) As TResult Implements IUIContext.Ask
        Return AnsiConsole.Ask($"[{prompt.Mood.ColorName}]{prompt.Text}[/]", defaultResult)
    End Function

    Public Function ReadKey() As String Implements IUIContext.ReadKey
        Do
            Dim key = AnsiConsole.Console.Input.ReadKey(True)
            If key.HasValue Then
                Return key?.Key.ToString()
            End If
        Loop
    End Function
End Class
