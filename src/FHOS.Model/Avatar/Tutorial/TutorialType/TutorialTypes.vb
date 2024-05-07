Public Module TutorialTypes
    Public Const StarSystemEntry As String = "StarSystemEntry"
    Public Const PlanetVicinityApproach As String = "PlanetVicinityApproach"
    Public Const StarVicinityApproach As String = "StarVicinityApproach"
    Public Const EnterPlanetOrbit As String = "EnterPlanetOrbit"
    Public Const EnterSatelliteOrbit As String = "EnterSatelliteOrbit"
    Public Const RefuelAtStar As String = "RefuelAtStar"
    Public Const OutOfFuel As String = "OutOfFuel"
    Public Const WormholeEntry As String = "WormholeEntry"
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, TutorialTypeDescriptor) =
        New Dictionary(Of String, TutorialTypeDescriptor) From
        {
            {StarSystemEntry, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{StarSystemEntry}")},
            {PlanetVicinityApproach, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{PlanetVicinityApproach}")},
            {StarVicinityApproach, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{StarVicinityApproach}")},
            {EnterPlanetOrbit, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{EnterPlanetOrbit}")},
            {EnterSatelliteOrbit, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{EnterSatelliteOrbit}")},
            {RefuelAtStar, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{RefuelAtStar}")},
            {OutOfFuel, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{OutOfFuel}")},
            {WormholeEntry, New TutorialTypeDescriptor(ignoreFlag:=$"IgnoreTutorial{WormholeEntry}")}
        }
End Module
