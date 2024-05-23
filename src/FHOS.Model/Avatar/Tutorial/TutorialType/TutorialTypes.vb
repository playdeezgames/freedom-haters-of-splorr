Public Module TutorialTypes
    Public ReadOnly StarSystemEntry As String = NameOf(StarSystemEntry)
    Public ReadOnly RefuelAtStar As String = NameOf(RefuelAtStar)
    Public ReadOnly OutOfFuel As String = NameOf(OutOfFuel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialTypeDescriptor) =
        New Dictionary(Of String, TutorialTypeDescriptor) From
        {
            {StarSystemEntry, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {RefuelAtStar, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{RefuelAtStar}")},
            {OutOfFuel, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{OutOfFuel}")}
        }
End Module
