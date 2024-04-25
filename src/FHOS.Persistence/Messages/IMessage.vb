Public Interface IMessage
    ReadOnly Property Header As String
    ReadOnly Property Lines As IEnumerable(Of (Text As String, Hue As Integer))
End Interface
