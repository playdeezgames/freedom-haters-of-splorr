Imports FHOS.Persistence

Friend Class ConditionalVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Private ReadOnly checkAvailability As Func(Of IActor, Boolean)

    Friend Sub New(verbType As String, text As String, checkAvailability As Func(Of IActor, Boolean))
        MyBase.New(verbType, text)
        Me.checkAvailability = checkAvailability
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        'Do Nothing
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return checkAvailability(actor)
    End Function
End Class
