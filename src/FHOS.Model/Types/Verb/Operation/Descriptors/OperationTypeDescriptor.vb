Imports FHOS.Persistence

Friend MustInherit Class OperationTypeDescriptor
    Friend ReadOnly Property OperationType As String
    Friend ReadOnly Property Text As String
    Friend ReadOnly Property Visible As Boolean
    Friend Sub New(
                  operationType As String,
                  text As String,
                  Optional visible As Boolean = True)
        Me.OperationType = operationType
        Me.Text = text
        Me.Visible = visible
    End Sub
    Friend MustOverride Function IsAvailable(actor As IActor) As Boolean
    Friend Sub Perform(actor As IActor)
        If IsAvailable(actor) Then
            OnPerform(actor)
        End If
    End Sub

    Protected MustOverride Sub OnPerform(actor As IActor)
End Class
