Friend Class VerbTypeDescriptor
    Friend ReadOnly Property Text As String
    Friend ReadOnly Property Visible As Boolean
    Sub New(text As String, Optional visible As Boolean = True)
        Me.Text = text
        Me.Visible = visible
    End Sub
End Class
