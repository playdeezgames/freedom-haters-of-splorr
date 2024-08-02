Public Interface IAvatarOperationsModel
    ReadOnly Property Available As IEnumerable(Of (Text As String, OperationType As String))
    ReadOnly Property HasAny As Boolean
    Sub Perform(operationType As String)
End Interface
