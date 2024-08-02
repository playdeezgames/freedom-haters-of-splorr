Imports FHOS.Persistence

Friend Class MoveOperationTypeDescriptor
    Inherits OperationTypeDescriptor
    ReadOnly Property facing As Integer

    Friend Sub New(operationType As String, text As String, facing As Integer)
        MyBase.New(
            operationType,
            text,
            False)
        Me.facing = facing
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        Move(actor, facing)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return CanMove(actor)
    End Function
End Class
