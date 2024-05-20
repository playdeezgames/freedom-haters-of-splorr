Imports FHOS.Persistence

Friend Class AlwaysAvailableVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String)
        MyBase.New(verbType, text)
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        'Do Nothing
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return True
    End Function
End Class
