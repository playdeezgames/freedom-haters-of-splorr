Public Interface IActorTutorial
    ReadOnly Property HasAny As Boolean
    ReadOnly Property Current As String
    Sub Dismiss()
    Sub Add(tutorial As String)
End Interface
