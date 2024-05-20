Imports FHOS.Persistence

Friend Class MoveVerbTypeDescriptor
    Inherits VerbTypeDescriptor
    ReadOnly Property facing As Integer

    Friend Sub New(verbType As String, text As String, facing As Integer)
        MyBase.New(
            verbType,
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
