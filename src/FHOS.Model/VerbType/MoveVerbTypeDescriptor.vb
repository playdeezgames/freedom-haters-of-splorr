Friend Class MoveVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String, facing As Integer)
        MyBase.New(
            verbType,
            text,
            Function(Actor)
                Return CanMove(Actor)
            End Function,
            Sub(actor)
                Move(actor, facing)
            End Sub,
            False)
    End Sub
End Class
