Public Interface IAvatarVerbsModel
    ReadOnly Property AvailableVerbs As IEnumerable(Of (Text As String, VerbType As String))
    ReadOnly Property HasVerbs As Boolean
    Sub DoVerb(verbType As String)
End Interface
