Public Module TutorialTypes
    Public ReadOnly StarSystemEntry As String = NameOf(StarSystemEntry)
    Public ReadOnly PlanetVicinityApproach As String = NameOf(PlanetVicinityApproach)
    Public ReadOnly StarVicinityApproach As String = NameOf(StarVicinityApproach)
    Public ReadOnly RefuelAtStar As String = NameOf(RefuelAtStar)
    Public ReadOnly OutOfFuel As String = NameOf(OutOfFuel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialTypeDescriptor) =
        New Dictionary(Of String, TutorialTypeDescriptor) From
        {
            {StarSystemEntry, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetVicinityApproach, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetVicinityApproach}")},
            {StarVicinityApproach, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{StarVicinityApproach}")},
            {RefuelAtStar, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{RefuelAtStar}")},
            {OutOfFuel, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{OutOfFuel}")}
        }
End Module
