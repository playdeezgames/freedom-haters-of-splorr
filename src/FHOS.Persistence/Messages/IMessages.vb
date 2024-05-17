Public Interface IMessages
    ReadOnly Property HasAny As Boolean
    ReadOnly Property Current As IMessage
    Sub Dismiss()
    Sub Add(
            header As String,
            ParamArray lines As (Text As String, Hue As Integer)())
End Interface
