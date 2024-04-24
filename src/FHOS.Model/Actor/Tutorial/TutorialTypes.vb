Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetVicinityApproach As String = "PlanetVicinityApproach"
    Public Const StarVicinityApproach As String = "StarVicinityApproach"
    Public Const PlanetLand As String = "PlanetLand"
    Public Const SatelliteApproach As String = "SatelliteApproach"
    Public Const RefuelAtStar As String = "RefuelAtStar"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetVicinityApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetVicinityApproach}")},
            {StarVicinityApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarVicinityApproach}")},
            {PlanetLand, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetLand}")},
            {SatelliteApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{SatelliteApproach}")},
            {RefuelAtStar, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{RefuelAtStar}")}
        }
End Module
