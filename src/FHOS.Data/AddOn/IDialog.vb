Public Interface IDialog
    ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
    ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Action))
End Interface
