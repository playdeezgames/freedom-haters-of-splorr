Public Interface IAvatarStateDialogModel
    ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
    ReadOnly Property Choices As IEnumerable(Of String)
    Sub Choose(choice As String)
    ReadOnly Property Prompt As String
End Interface
