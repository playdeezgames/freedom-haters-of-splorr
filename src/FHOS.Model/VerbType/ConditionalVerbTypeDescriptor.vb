Imports FHOS.Persistence

Friend Class ConditionalVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String, checkAvailability As Func(Of IActor, Boolean))
        MyBase.New(verbType, text, checkAvailability)
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        'Do Nothing
    End Sub
End Class
