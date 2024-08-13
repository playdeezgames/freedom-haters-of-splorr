Public Interface IUIContext
    Sub Clear()
    Sub ClearImmediate()
    Function Choose(Of TResult)(
                               prompt As (Mood As Mood, Text As String),
                               ParamArray choices As (Text As String, Value As TResult)()) As TResult
    Function Choose(
                    prompt As (Mood As Mood, Text As String),
                    ParamArray choices As String()) As String
    Function Confirm(prompt As (Mood As Mood, Text As String)) As Boolean
    Function Ask(Of TResult)(prompt As (Mood As Mood, Text As String), defaultResult As TResult) As TResult
    Sub Message(prompt As (Mood As Mood, Text As String))
    Sub WriteLine(ParamArray lines As (Mood As Mood, Text As String)())
    Sub WriteLineImmediate(ParamArray lines As (Mood As Mood, Text As String)())
    Sub Write(stuff As (Mood As Mood, Text As String))
    Sub WriteExceptionImmediate(ex As Exception)
    Sub WriteFigletImmediate(figlet As (Mood As Mood, Text As String))
    Function ReadKey() As String
End Interface
