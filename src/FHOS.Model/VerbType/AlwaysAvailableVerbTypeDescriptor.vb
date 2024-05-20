Imports FHOS.Persistence

Friend Class AlwaysAvailableVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String)
        MyBase.New(verbType, text, Function(x) True, Sub() Return)
    End Sub
End Class
