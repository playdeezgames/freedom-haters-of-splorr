Imports FHOS.Persistence

Public Module VerbTypes
    Public ReadOnly ApproachPlanetVicinity As String = NameOf(ApproachPlanetVicinity)
    Public ReadOnly ApproachStarVicinity As String = NameOf(ApproachStarVicinity)
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly EnterStarSystem As String = NameOf(EnterStarSystem)
    Public ReadOnly KnownPlaces As String = NameOf(KnownPlaces)
    Public ReadOnly MoveRight As String = NameOf(MoveRight)
    Public ReadOnly MoveUp As String = NameOf(MoveUp)
    Public ReadOnly MoveDown As String = NameOf(MoveDown)
    Public ReadOnly MoveLeft As String = NameOf(MoveLeft)
    Public ReadOnly Refuel As String = NameOf(Refuel)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Public ReadOnly Status As String = NameOf(Status)
    Public ReadOnly Crew As String = NameOf(Crew)
    Public ReadOnly Vessel As String = NameOf(Vessel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, VerbTypeDescriptor) =
        New List(Of VerbTypeDescriptor) From
        {
            New MoveVerbTypeDescriptor(MoveUp, "Move Up", Facing.Up),
            New MoveVerbTypeDescriptor(MoveRight, "Move Right", Facing.Right),
            New MoveVerbTypeDescriptor(MoveDown, "Move Down", Facing.Down),
            New MoveVerbTypeDescriptor(MoveLeft, "Move Left", Facing.Left),
            New ApproachVerbTypeDescriptor(EnterStarSystem, "Enter Star System", PlaceTypes.StarSystem),
            New ApproachVerbTypeDescriptor(ApproachPlanetVicinity, "Approach Planet", PlaceTypes.PlanetVicinity),
            New ApproachVerbTypeDescriptor(ApproachStarVicinity, "Approach Star", PlaceTypes.StarVicinity),
            New RefuelVerbTypeDescriptor(),
            New ConditionalVerbTypeDescriptor(KnownPlaces, "Known Places...", Function(Actor) Actor.KnownPlaces.HasAny),
            New DistressSignalVerbTypeDescriptor(),
            New AlwaysAvailableVerbTypeDescriptor(Status, "Status"),
            New AlwaysAvailableVerbTypeDescriptor(SPLORRPedia, "SPLORR!!Pedia"),
            New ConditionalVerbTypeDescriptor(Crew, "Crew...", Function(Actor) Actor.Family.HasChildren),
            New ConditionalVerbTypeDescriptor(Vessel, "Vessel...", Function(Actor) Actor.Family.Parent IsNot Nothing)
        }.ToDictionary(Function(x) x.VerbType, Function(x) x)
End Module
