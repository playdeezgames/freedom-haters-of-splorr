Public Interface IDialog
    ReadOnly Property DialogType As DialogType
    ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
    ReadOnly Property LegacyChoices As IEnumerable(Of (Text As String, Value As Func(Of IDialog)))
    ReadOnly Property Menu As IEnumerable(Of String)
    ReadOnly Property DefaultInput As String
    ReadOnly Property Prompt As String
    Function Choose(choice As String) As IDialog
End Interface
