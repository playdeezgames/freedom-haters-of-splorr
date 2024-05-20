Imports FHOS.Persistence

Friend MustInherit Class VerbTypeDescriptor
    Friend ReadOnly Property VerbType As String
    Friend ReadOnly Property Text As String
    Friend ReadOnly Property Visible As Boolean
    Private ReadOnly checkAvailability As Func(Of IActor, Boolean)
    Friend Sub New(
                  verbType As String,
                  text As String,
                  checkAvailability As Func(Of IActor, Boolean),
                  Optional visible As Boolean = True)
        Me.VerbType = verbType
        Me.Text = text
        Me.Visible = visible
        Me.checkAvailability = checkAvailability
    End Sub
    Friend Function IsAvailable(actor As IActor) As Boolean
        Return checkAvailability(actor)
    End Function
    Friend MustOverride Sub Perform(actor As IActor)
End Class
