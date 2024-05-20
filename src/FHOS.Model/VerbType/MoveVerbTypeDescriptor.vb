Imports FHOS.Persistence

Friend Class MoveVerbTypeDescriptor
    Inherits VerbTypeDescriptor
    ReadOnly Property facing As Integer

    Friend Sub New(verbType As String, text As String, facing As Integer)
        MyBase.New(
            verbType,
            text,
            Function(Actor)
                Return CanMove(Actor)
            End Function,
            False)
        Me.facing = facing
    End Sub

    Friend Overrides Sub Perform(actor As Persistence.IActor)
        Move(actor, Facing)
    End Sub
End Class
