Public Interface IUIContext
    Sub Clear()
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
    Sub Write(stuff As (Mood As Mood, Text As String))
    Sub WriteException(ex As Exception)
    Sub WriteFiglet(figlet As (Mood As Mood, Text As String))
    Function ReadKey() As String
End Interface
