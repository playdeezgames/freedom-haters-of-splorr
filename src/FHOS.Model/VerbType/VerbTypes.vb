Public Module VerbTypes
    Public ReadOnly EnterStarSystem As String = NameOf(EnterStarSystem)
    Public ReadOnly ApproachPlanetVicinity As String = NameOf(ApproachPlanetVicinity)
    Public ReadOnly ApproachStarVicinity As String = NameOf(ApproachStarVicinity)
    Public ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Public ReadOnly Refuel As String = NameOf(Refuel)
    Public ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Public ReadOnly EnterOrbit As String = NameOf(EnterOrbit)
    Public ReadOnly KnownPlaces As String = NameOf(KnownPlaces)
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly Status As String = NameOf(Status)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, VerbTypeDescriptor) =
        New Dictionary(Of String, VerbTypeDescriptor) From
        {
            {EnterStarSystem, New VerbTypeDescriptor("Enter Star System")},
            {ApproachPlanetVicinity, New VerbTypeDescriptor("Approach Planet")},
            {ApproachStarVicinity, New VerbTypeDescriptor("Approach Star")},
            {RefillOxygen, New VerbTypeDescriptor("Refill Oxygen")},
            {Refuel, New VerbTypeDescriptor("Refuel")},
            {EnterWormhole, New VerbTypeDescriptor("Enter Wormhole")},
            {EnterOrbit, New VerbTypeDescriptor("Enter Orbit")},
            {KnownPlaces, New VerbTypeDescriptor("Known Places...")},
            {DistressSignal, New VerbTypeDescriptor("Signal Distress")},
            {Status, New VerbTypeDescriptor("Status")},
            {SPLORRPedia, New VerbTypeDescriptor("SPLORR!!Pedia")}
        }
End Module
