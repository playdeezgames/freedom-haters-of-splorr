Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")}
        }
End Module
