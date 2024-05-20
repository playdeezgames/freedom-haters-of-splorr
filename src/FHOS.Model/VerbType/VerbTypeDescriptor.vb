Imports FHOS.Persistence

Friend MustInherit Class VerbTypeDescriptor
    Friend ReadOnly Property VerbType As String
    Friend ReadOnly Property Text As String
    Friend ReadOnly Property Visible As Boolean
    Friend Sub New(
                  verbType As String,
                  text As String,
                  Optional visible As Boolean = True)
        Me.VerbType = verbType
        Me.Text = text
        Me.Visible = visible
    End Sub
    Friend MustOverride Function IsAvailable(actor As IActor) As Boolean
    Friend MustOverride Sub Perform(actor As IActor)
End Class
