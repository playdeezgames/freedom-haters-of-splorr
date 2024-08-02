Imports FHOS.Persistence

Friend Class AlwaysAvailableOperationTypeDescriptor
    Inherits OperationTypeDescriptor

    Friend Sub New(operationType As String, text As String)
        MyBase.New(operationType, text)
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        'Do Nothing
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return True
    End Function
End Class
