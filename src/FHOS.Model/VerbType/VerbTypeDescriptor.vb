Imports FHOS.Persistence

Friend MustInherit Class VerbTypeDescriptor
    Friend ReadOnly Property VerbType As String
    Friend ReadOnly Property Text As String
    Friend ReadOnly Property Visible As Boolean
    Private ReadOnly checkAvailability As Func(Of IActor, Boolean)
    Private ReadOnly doPerform As Action(Of IActor)
    Friend Sub New(
                  verbType As String,
                  text As String,
                  checkAvailability As Func(Of IActor, Boolean),
                  doPerform As Action(Of IActor),
                  Optional visible As Boolean = True)
        Me.VerbType = verbType
        Me.Text = text
        Me.Visible = visible
        Me.checkAvailability = checkAvailability
        Me.doPerform = doPerform
    End Sub
    Friend Function IsAvailable(actor As IActor) As Boolean
        Return checkAvailability(actor)
    End Function
    Friend Sub Perform(actor As IActor)
        doPerform(actor)
    End Sub
End Class
