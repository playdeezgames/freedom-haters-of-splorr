Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetaryEntry As String = "PlanetaryEntry"
    Public Const StarApproach As String = "StarApproach"
    Public Const PlanetLand As String = "PlanetLand"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetaryEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetaryEntry}")},
            {StarApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarApproach}")},
            {PlanetLand, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetLand}")}
        }
End Module
