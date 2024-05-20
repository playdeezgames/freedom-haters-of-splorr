Imports FHOS.Persistence

Friend Class AlwaysAvailableVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String)
        MyBase.New(verbType, text, Function(x) True)
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        'Do Nothing
    End Sub
End Class
