Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetaryEntry As String = "PlanetaryEntry"
    Public Const StarApproach As String = "StarApproach"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetaryEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetaryEntry}")},
            {StarApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarApproach}")}
        }
End Module
