Public Module TutorialTypes
    Public ReadOnly StarSystemEntry As String = NameOf(StarSystemEntry)
    Public ReadOnly PlanetVicinityApproach As String = NameOf(PlanetVicinityApproach)
    Public ReadOnly StarVicinityApproach As String = NameOf(StarVicinityApproach)
    Public ReadOnly EnterPlanetOrbit As String = NameOf(EnterPlanetOrbit)
    Public ReadOnly EnterSatelliteOrbit As String = NameOf(EnterSatelliteOrbit)
    Public ReadOnly RefuelAtStar As String = NameOf(RefuelAtStar)
    Public ReadOnly OutOfFuel As String = NameOf(OutOfFuel)
    Public ReadOnly WormholeEntry As String = NameOf(WormholeEntry)

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
