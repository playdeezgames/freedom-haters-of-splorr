Public Interface IAvatarVerbsModel
    ReadOnly Property Available As IEnumerable(Of (Text As String, VerbType As String))
    ReadOnly Property HasAny As Boolean
    Sub Perform(verbType As String)
End Interface
