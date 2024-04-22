Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetaryEntry As String = "PlanetaryEntry"
    Public Const StarApproach As String = "StarApproach"
    Public Const PlanetLand As String = "PlanetLand"
    Public Const SatelliteApproach As String = "SatelliteApproach"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetaryEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetaryEntry}")},
            {StarApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarApproach}")},
            {PlanetLand, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetLand}")},
            {SatelliteApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{SatelliteApproach}")}
        }
End Module
