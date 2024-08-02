Imports FHOS.Persistence

Friend Class ConditionalOperationTypeDescriptor
    Inherits OperationTypeDescriptor

    Private ReadOnly checkAvailability As Func(Of IActor, Boolean)

    Friend Sub New(operationType As String, text As String, checkAvailability As Func(Of IActor, Boolean))
        MyBase.New(operationType, text)
        Me.checkAvailability = checkAvailability
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        'Do Nothing
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return checkAvailability(actor)
    End Function
End Class
