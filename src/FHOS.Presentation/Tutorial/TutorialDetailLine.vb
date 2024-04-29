Public Class TutorialDetailLine
    ReadOnly Property Text As String
    ReadOnly Property Hue As Integer
    Sub New(text As String, hue As Integer)
        Me.Text = text
        Me.Hue = hue
    End Sub
End Class
