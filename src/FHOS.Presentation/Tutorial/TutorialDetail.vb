Friend Class TutorialDetail
    Sub New(header As String, ParamArray lines As TutorialDetailLine())
        Me.Header = header
        Me.Lines = lines
    End Sub
    ReadOnly Property Header As String
    ReadOnly Property Lines As IEnumerable(Of TutorialDetailLine)
End Class
