Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetVicinityApproach As String = "PlanetVicinityApproach"
    Public Const StarVicinityApproach As String = "StarVicinityApproach"
    Public Const EnterPlanetOrbit As String = "EnterPlanetOrbit"
    Public Const SatelliteApproach As String = "SatelliteApproach"
    Public Const RefuelAtStar As String = "RefuelAtStar"
    Public Const OutOfFuel As String = "OutOfFuel"
    Public Const WormholeEntry As String = "WormholeEntry"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialDescriptor) =
        New Dictionary(Of String, TutorialDescriptor) From
        {
            {StarSystemEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetVicinityApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetVicinityApproach}")},
            {StarVicinityApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{StarVicinityApproach}")},
            {EnterPlanetOrbit, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{EnterPlanetOrbit}")},
            {SatelliteApproach, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{SatelliteApproach}")},
            {RefuelAtStar, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{RefuelAtStar}")},
            {OutOfFuel, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{OutOfFuel}")},
            {WormholeEntry, New TutorialDescriptor(ignoreFlag:=$"IgnoreTutorial{WormholeEntry}")}
        }
End Module
