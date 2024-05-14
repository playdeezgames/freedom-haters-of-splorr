Public Module VerbTypes
    Public ReadOnly ApproachPlanetVicinity As String = NameOf(ApproachPlanetVicinity)
    Public ReadOnly ApproachStarVicinity As String = NameOf(ApproachStarVicinity)
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly EnterOrbit As String = NameOf(EnterOrbit)
    Public ReadOnly EnterStarSystem As String = NameOf(EnterStarSystem)
    Public ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Public ReadOnly KnownPlaces As String = NameOf(KnownPlaces)
    Public ReadOnly MoveRight As String = NameOf(MoveRight)
    Public ReadOnly MoveUp As String = NameOf(MoveUp)
    Public ReadOnly MoveDown As String = NameOf(MoveDown)
    Public ReadOnly MoveLeft As String = NameOf(MoveLeft)
    Public ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Public ReadOnly Refuel As String = NameOf(Refuel)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Public ReadOnly Status As String = NameOf(Status)
    Public ReadOnly Crew As String = NameOf(Crew)
    Public ReadOnly Vessel As String = NameOf(Vessel)

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
            {MoveUp, New VerbTypeDescriptor("Move Up", visible:=False)},
            {MoveRight, New VerbTypeDescriptor("Move Right", visible:=False)},
            {MoveDown, New VerbTypeDescriptor("Move Down", visible:=False)},
            {MoveLeft, New VerbTypeDescriptor("Move Left", visible:=False)},
            {SPLORRPedia, New VerbTypeDescriptor("SPLORR!!Pedia")},
            {Crew, New VerbTypeDescriptor("Crew...")},
            {Vessel, New VerbTypeDescriptor("Vessel...")}
        }
End Module
