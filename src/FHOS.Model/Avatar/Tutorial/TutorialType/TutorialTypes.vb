Public Module TutorialTypes
    Public ReadOnly OutOfFuel As String = NameOf(OutOfFuel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialTypeDescriptor) =
        New Dictionary(Of String, TutorialTypeDescriptor) From
        {
            {OutOfFuel, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{OutOfFuel}")}
        }
End Module
